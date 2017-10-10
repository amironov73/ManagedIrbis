// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftNode.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Pft.Infrastructure.Walking;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable DoNotCallOverridableMethodsInConstructor

namespace ManagedIrbis.Pft.Infrastructure
{
    using Diagnostics;

    /// <summary>
    /// AST item
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftNode
        : IVerifiable,
        ICloneable
    {
        #region Events

        /// <summary>
        /// Вызывается непосредственно перед выполнением.
        /// </summary>
        public event EventHandler<PftDebugEventArgs> BeforeExecution;

        /// <summary>
        /// Вызывается непосредственно после выполнения.
        /// </summary>
        public event EventHandler<PftDebugEventArgs> AfterExecution;

        #endregion

        #region Properties

        /// <summary>
        /// Parent node.
        /// </summary>
        [CanBeNull]
        public PftNode Parent { get; internal set; }

        /// <summary>
        /// Breakpoint.
        /// </summary>
        public bool Breakpoint { get; set; }

        /// <summary>
        /// Список потомков. Может быть пустым.
        /// </summary>
        public virtual IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_children, null))
                {
                    _children = new PftNodeCollection(this);
                }

                return _children;
            }
            protected set
            {
                _children = (PftNodeCollection)value;
            }
        }

        /// <summary>
        /// Column number.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Номер строки, на которой в скрипте расположена
        /// соответствующая конструкция языка.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        public virtual string Text { get; set; }

        /// <summary>
        /// Whether the node is complex expression?
        /// </summary>
        public virtual bool ComplexExpression { get { return false; } }

        /// <summary>
        /// Whether the node is constant expression?
        /// </summary>
        public virtual bool ConstantExpression { get { return false; } }

        /// <summary>
        /// Node uses extended syntax?
        /// </summary>
        public virtual bool ExtendedSyntax { get { return false; } }

        /// <summary>
        /// Help for the node.
        /// </summary>
        [CanBeNull]
        public virtual string Help { get { return null; } }

        /// <summary>
        /// Whether the node requires server connection to evaluate.
        /// </summary>
        public virtual bool RequiresConnection { get { return true; } }

        /// <summary>
        /// Kind of the node.
        /// </summary>
        public virtual PftNodeKind Kind { get { return PftNodeKind.None; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNode()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNode
            (
                [NotNull] PftToken token
            )
        {
            Code.NotNull(token, "token");

            LineNumber = token.Line;
            Column = token.Column;
            Text = token.Text;
        }

        #endregion

        #region Private members

        private PftNodeCollection _children;

        /// <summary>
        /// Check deserialization result.
        /// </summary>
        internal virtual void CompareNode
            (
                [NotNull] PftNode otherNode
            )
        {
            bool result = Column == otherNode.Column
                          && LineNumber == otherNode.LineNumber;

            if (result && ShouldSerializeText())
            {
                result = Text == otherNode.Text;
            }

            if (result && ShouldSerializeChildren())
            {
                result = Children.Count == otherNode.Children.Count;
                if (result)
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        PftNode our = Children[i];
                        PftNode their = otherNode.Children[i];

                        if (!ReferenceEquals(our.GetType(), their.GetType()))
                        {
                            throw new PftSerializationException
                                (
                                    "Expecting " + our.GetType()
                                    + ", got " + their.GetType()
                                );
                        }

                        our.CompareNode(their);
                    }
                }
            }

            if (!result)
            {
                throw new PftSerializationException
                    (
                        "CompareNode failed with " + GetType()
                    );
            }
        }

        /// <summary>
        /// Deserialize AST.
        /// </summary>
        protected internal virtual void Deserialize
            (
                [NotNull] BinaryReader reader
            )
        {
            Column = reader.ReadPackedInt32();
            LineNumber = reader.ReadPackedInt32();
            if (ShouldSerializeText())
            {
                Text = reader.ReadNullableString();
            }

            if (ShouldSerializeChildren())
            {
                PftSerializer.Deserialize(reader, Children);
            }
        }

        /// <summary>
        /// After execution.
        /// </summary>
        protected void OnAfterExecution
            (
                [CanBeNull] PftContext context
            )
        {
            var handler = AfterExecution;
            if (handler != null)
            {
                PftDebugEventArgs eventArgs = new PftDebugEventArgs
                    (
                        context,
                        this
                    );
                handler(this, eventArgs);
            }

            if (Breakpoint
                && !ReferenceEquals(context, null)
                && !ReferenceEquals(context.Debugger, null))
            {
                PftDebugEventArgs eventArgs = new PftDebugEventArgs
                    (
                        context,
                        this
                    );
                context.Debugger.Activate(eventArgs);
            }
        }

        /// <summary>
        /// Before execution.
        /// </summary>
        protected void OnBeforeExecution
            (
                [CanBeNull] PftContext context
            )
        {
            var handler = BeforeExecution;
            if (handler != null)
            {
                PftDebugEventArgs eventArgs = new PftDebugEventArgs
                    (
                        context,
                        this
                    );
                handler(this, eventArgs);
            }

            if (Breakpoint)
            {
                if (!ReferenceEquals(context, null))
                {
                    context.ActivateDebugger(this);
                }
            }
        }

        /// <summary>
        /// Serialize AST.
        /// </summary>
        protected internal virtual void Serialize
            (
                [NotNull] BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Column)
                .WritePackedInt32(LineNumber);
            if (ShouldSerializeText())
            {
                writer.WriteNullable(Text);
            }

            if (ShouldSerializeChildren())
            {
                PftSerializer.Serialize(writer, Children);
            }
        }

        /// <summary>
        /// Should serialize <see cref="Children"/> property?
        /// </summary>
        protected internal virtual bool ShouldSerializeChildren()
        {
            PftNodeCollection children = Children as PftNodeCollection;

            return !ReferenceEquals(children, null);
        }

        /// <summary>
        /// Should serialize <see cref="Text"/> property?
        /// </summary>
        protected internal virtual bool ShouldSerializeText()
        {
            return true;
        }

        /// <summary>
        /// Simplify type name.
        /// </summary>
        [NotNull]
        protected internal static string SimplifyTypeName
            (
                [NotNull] string typeName
            )
        {
            return typeName.StartsWith("Pft")
                ? typeName.Substring(3)
                : typeName;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Accept the visitor.
        /// </summary>
        /// <returns>
        /// <c>true</c> means "continue".
        /// </returns>
        public virtual bool AcceptVisitor
            (
                [NotNull] PftVisitor visitor
            )
        {
            Code.NotNull(visitor, "visitor");

            if (!visitor.VisitNode(this))
            {
                return false;
            }

            foreach (PftNode child in Children)
            {
                if (!child.AcceptVisitor(visitor))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compiles the node.
        /// </summary>
        public virtual void Compile
            (
                [NotNull] PftCompiler compiler
            )
        {
            Code.NotNull(compiler, "compiler");

            bool flag = ShouldSerializeChildren();
            if (flag)
            {
                compiler.CompileNodes(Children);
            }

            compiler.StartMethod(this);
            if (flag)
            {
                compiler.CallNodes(Children);
            }
            compiler.EndMethod(this);

            compiler.MarkReady(this);
        }

        /// <summary>
        /// Собственно форматирование.
        /// Включает в себя результат
        /// форматирования всеми потомками.
        /// </summary>
        public virtual void Execute
            (
                [NotNull] PftContext context
            )
        {
            OnBeforeExecution(context);

            foreach (PftNode child in Children)
            {
                child.Execute(context);
            }

            OnAfterExecution(context);
        }

        /// <summary>
        /// Список полей, задействованных в форматировании
        /// данным элементом и всеми его потомками, включая
        /// косвенных.
        /// </summary>
        [NotNull]
        public virtual int[] GetAffectedFields()
        {
            int[] result = new int[0];

            foreach (PftNode child in Children)
            {
                int[] sub = child.GetAffectedFields();
                if (sub.Length != 0)
                {
                    result = result
                        .Union(sub)
                        .Distinct()
                        .ToArray();
                }
            }

            return result;
        }

        /// <summary>
        /// Получение списка потомков (как прямых,
        /// так и косвенных) определенного типа.
        /// </summary>
        [NotNull]
        public NonNullCollection<T> GetDescendants<T>()
            where T : PftNode
        {
            NonNullCollection<T> result = new NonNullCollection<T>();

            foreach (PftNode child in Children)
            {
                T item = child as T;
                if (item != null)
                {
                    result.Add(item);
                }
                result.AddRange(child.GetDescendants<T>());
            }

            return result;
        }

        /// <summary>
        /// Построение массива потомков-листьев 
        /// (т. е. не имеющих собственных потомков).
        /// </summary>
        /// <remarks>Если у узла нет потомков,
        /// он возвращает массив из одного элемента:
        /// самого себя.</remarks>
        [NotNull]
        public PftNode[] GetLeafs()
        {
            if (Children.Count == 0)
            {
                return new[] { this };
            }

            PftNode[] result = Children
                .SelectMany(child => child.GetLeafs())
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get node info for debugger.
        /// </summary>
        [NotNull]
        public virtual PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = SimplifyTypeName(GetType().Name),
                Node = this,
                Value = Text
            };

            foreach (PftNode child in Children)
            {
                PftNodeInfo info = child.GetNodeInfo();
                result.Children.Add(info);
            }

            return result;
        }

        /// <summary>
        /// Оптимизация дерева потомков.
        /// На данный момент не реализована.
        /// </summary>
        /// <remarks>Если возвращает <c>null</c>,
        /// это означает, что данный узел и всех
        /// его потомков можно безболезненно удалить.
        /// </remarks>
        [CanBeNull]
        public virtual PftNode Optimize()
        {
            PftNodeCollection children = Children as PftNodeCollection;
            if (!ReferenceEquals(children, null))
            {
                children.Optimize();
            }

            return this;
        }

        /// <summary>
        /// Поддерживает ли многопоточность,
        /// т. е. может ли быть запущен одновременно
        /// с соседними элементами.
        /// </summary>
        /// <remarks>
        /// На данный момент многопоточность
        /// не поддерживается.
        /// </remarks>
        public virtual bool SupportsMultithreading()
        {
            return Children.Count != 0
                && Children.All
                (
                    child => child.SupportsMultithreading()
                );
        }

        /// <summary>
        /// Формирование исходного текста по AST.
        /// Применяется, например, для красивой 
        /// распечатки программы на языке PFT.
        /// </summary>
        public virtual void PrettyPrint
            (
                [NotNull] PftPrettyPrinter printer
            )
        {
            Code.NotNull(printer, "printer");

            if (ShouldSerializeChildren())
            {
                printer.WriteNodes(Children);
            }
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            PftNode result = (PftNode)MemberwiseClone();
            result.Parent = null;

            PftNodeCollection children
                = Children as PftNodeCollection;
            if (!ReferenceEquals(children, null))
            {
                result.Children = children.CloneNodes(result);
            }

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            bool result = Children.All
                (
                    child => child.Verify(throwOnError)
                );

            if (!result)
            {
                Log.Error
                    (
                        "PftNode::Verify: "
                        + "verification failed"
                    );

                if (throwOnError)
                {
                    throw new VerificationException();
                }
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object other
            )
        {
            return ReferenceEquals(this, other);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Column;
                hashCode = (hashCode * 397) ^ LineNumber;
                hashCode = (hashCode * 397) ^
                    (
                        Text != null
                        ? Text.GetHashCode()
                        : 0
                    );

                return hashCode;
            }
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (PftNode child in Children)
            {
                result.Append(child);
            }

            return result.ToString();
        }

        #endregion
    }
}
