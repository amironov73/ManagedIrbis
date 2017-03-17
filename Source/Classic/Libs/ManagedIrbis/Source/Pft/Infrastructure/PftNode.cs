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
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    using Diagnostics;

    /// <summary>
    /// AST item
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftNode
        : IHandmadeSerializable,
        ITreeSerialize,
        IVerifiable
#if !NETCORE && !UAP && !WIN81 && !PORTABLE
        , ICloneable
#endif
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
        /// Breakpoint.
        /// </summary>
        public bool Breakpoint { get; set; }

        /// <summary>
        /// Список потомков. Может быть пустым.
        /// </summary>
        public virtual IList<PftNode> Children
        {
            get;
            protected set;
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
        /// Node uses extended syntax?
        /// </summary>
        public virtual bool ExtendedSyntax { get { return false; } }

        /// <summary>
        /// Help for the node.
        /// </summary>
        [CanBeNull]
        public virtual string Help { get { return null; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNode()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            Children = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNode
            (
                [NotNull] PftToken token
            )
            : this()
        {
            Code.NotNull(token, "token");

            LineNumber = token.Line;
            Column = token.Column;
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            Text = token.Text;
        }

        #endregion

        #region Private members

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
        /// Simplify type name.
        /// </summary>
        [NotNull]
        protected static string SimplifyTypeName
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
        /// Собственно форматирование.
        /// Включает в себя результат
        /// форматирования всеми потомками.
        /// </summary>
        public virtual void Execute
            (
                PftContext context
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
        public virtual string[] GetAffectedFields()
        {
            string[] result = new string[0];

            foreach (PftNode child in Children)
            {
                string[] sub = child.GetAffectedFields();
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
            if (Children.Count == 1)
            {
                return Children[0].Optimize();
            }

            return this;
        }

        /// <summary>
        /// Поддерживает ли многопоточность,
        /// т. е. может ли быть запущен одновременно
        /// с соседними элементами.
        /// </summary>
        public virtual bool SupportsMultithreading()
        {
            return (Children.Count != 0)
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
        public virtual void Write
            (
                [NotNull] StreamWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            foreach (PftNode child in Children)
            {
                child.Write(writer);
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");
        }

        /// <inheritdoc/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");
        }

        #endregion

        #region ITreeSerialize members

        /// <inheritdoc/>
        public void DeserializeTree
            (
                BinaryReader reader
            )
        {
#if NETCORE

            throw new NotImplementedException();

#else

            Code.NotNull(reader, "reader");

            RestoreFromStream(reader);

            Children.Clear();
            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                string typeName = reader.ReadString();
                Type type = Type.GetType(typeName, true);
                PftNode child = (PftNode)Activator.CreateInstance(type);
                Children.Add(child);
            }
#endif
        }

        /// <inheritdoc/>
        public void SerializeTree
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

#if NETCORE

            throw new NotImplementedException();

#else

            SaveToStream(writer);

            int count = Children.Count;
            writer.WritePackedInt32(count);
            foreach (PftNode child in Children)
            {
                Type type = child.GetType();
                string typeName = type.AssemblyQualifiedName
                    .ThrowIfNull("typeName");
                writer.Write(typeName);
                child.SerializeTree(writer);
            }

#endif
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Семантическая валидация поддерева.
        /// На данный момент не реализована
        /// </summary>
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            bool result = Children.All
                (
                    child => child.Verify(throwOnError)
                );

            if (!result && throwOnError)
            {
                throw new ArgumentException();
            }

            return result;
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc />
        public virtual object Clone()
        {
            PftNode result = (PftNode) MemberwiseClone();

            NonNullCollection<PftNode> children 
                = Children as NonNullCollection<PftNode>;
            if (!ReferenceEquals(children, null))
            {
                result.Children = children.CloneNodes();
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc />
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
