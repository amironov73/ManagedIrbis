/* PftParser.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Text;
using AM.Text.Tokenizer;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    using Ast;

    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Parser for PFT language.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed partial class PftParser
    {
        #region Properties

        /// <summary>
        /// Token list.
        /// </summary>
        [NotNull]
        public PftTokenList Tokens { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public PftParser
            (
                [NotNull] PftTokenList tokens
            )
        {
            Code.NotNull(tokens, "tokens");
            Tokens = tokens;

            _procedures = new PftProcedureManager();
            CreateTokenMap();
        }

        #endregion

        #region Private members

        //================================================================
        // Parsing
        //================================================================

        private PftA ParseA()
        {
            PftA result = new PftA(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext(PftTokenKind.V);
            PftField field = (PftField)ParseField();
            if (ReferenceEquals(field, null))
            {
                throw new PftSyntaxException(Tokens.Current);
            }
            result.Field = field;
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            return MoveNext(result);
        }

        //=================================================

        private PftAssignment ParseAssignment
            (
                [NotNull] PftAssignment result,
                [NotNull] PftTokenList tokens
            )
        {
            PftTokenList saveList = Tokens;
            Tokens = tokens;
            int position = Tokens.SavePosition();

            try
            {
                PftNode node;
                try
                {
                    node = ParseArithmetic();
                    result.Children.Add(node);
                    if (!Tokens.IsEof)
                    {
                        throw new PftSyntaxException();
                    }
                    result.IsNumeric = true;
                }
                catch
                {
                    result.Children.Clear();
                    Tokens.RestorePosition(position);
                    while (!Tokens.IsEof)
                    {
                        node = ParseNext();
                        result.Children.Add(node);
                    }
                }
            }
            finally
            {
                Tokens = saveList;
            }

            return result;
        }

        //=================================================

        private PftNode ParseAt()
        {
            return MoveNext(new PftInclude(Tokens.Current));
        }

        //=================================================

        private PftNode ParseBang()
        {
            return MoveNext(new PftBang(Tokens.Current));
        }

        //=================================================

        private PftBlank ParseBlank()
        {
            PftBlank result = new PftBlank(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseBreak()
        {
            return MoveNext(new PftBreak(Tokens.Current));
        }

        //=================================================

        private PftNode ParseC()
        {
            return MoveNext(new PftC(Tokens.Current));
        }

        //=================================================

        private PftNode ParseCodeBlock()
        {
            return MoveNext(new PftCodeBlock(Tokens.Current));
        }

        //=================================================

        private PftNode ParseComma()
        {
            return MoveNext(new PftComma(Tokens.Current));
        }

        //=================================================

        private PftNode ParseComment()
        {
            return MoveNext(new PftComment(Tokens.Current));
        }

        //=================================================

        private PftNode ParseConditionalLiteral()
        {
            return MoveNext
                (
                    new PftConditionalLiteral(Tokens.Current)
                );
        }

        //=================================================

        private PftNode ParseEat()
        {
            PftEat result = new PftEat(Tokens.Current);
            Tokens.MoveNext();

            bool ok = false;
            while (!Tokens.IsEof)
            {
                if (Tokens.Current.Kind == PftTokenKind.EatClose)
                {
                    ok = true;
                    Tokens.MoveNext();
                    break;
                }

                PftNode node = ParseNext();
                result.Children.Add(node);
            }

            if (!ok)
            {
                throw new PftSyntaxException(Tokens);
            }

            return result;
        }


        //=================================================

        private PftEmpty ParseEmpty()
        {
            PftEmpty result = new PftEmpty(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseF()
        {
            PftF result = new PftF(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            result.Argument1 = ParseArithmetic
                (
                    PftTokenKind.Comma,
                    PftTokenKind.RightParenthesis
                );
            if (Tokens.IsEof)
            {
                throw new PftSyntaxException(Tokens);
            }
            if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
            {
                if (Tokens.Current.Kind != PftTokenKind.Comma)
                {
                    throw new PftSyntaxException(Tokens.Current);
                }
                Tokens.RequireNext();
                result.Argument2 = ParseNumber();
                if (Tokens.IsEof)
                {
                    throw new PftSyntaxException(Tokens);
                }
                if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
                {
                    if (Tokens.Current.Kind != PftTokenKind.Comma)
                    {
                        throw new PftSyntaxException(Tokens.Current);
                    }
                    Tokens.RequireNext();
                    result.Argument3 = ParseNumber();
                }
            }
            if (Tokens.IsEof)
            {
                throw new PftSyntaxException(Tokens);
            }
            if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
            {
                throw new PftSyntaxException(Tokens.Current);
            }
            Tokens.MoveNext();

            return result;
        }

        //=================================================

        private PftNode ParseF2()
        {
            PftF2 result = new PftF2(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            result.Number = ParseArithmetic
                (
                    PftTokenKind.Comma
                );
            if (Tokens.IsEof
                || Tokens.Current.Kind != PftTokenKind.Comma
               )
            {
                throw new PftSyntaxException(Tokens);
            }
            Tokens.RequireNext();
            PftTokenList formatTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
                .ThrowIfNull("formatTokens");
            ChangeContext
                (
                    result.Format,
                    formatTokens
                );
            Tokens.MoveNext();

            return result;
        }

        //=================================================

        /// <summary>
        /// For loop.
        /// </summary>
        /// <example>
        /// for $x=0; $x &lt; 10; $x = $x+1;
        /// do
        ///     $x, ') ',
        ///     'Прикольно же!'
        ///     #
        /// end
        /// </example>
        private PftNode ParseFor()
        {
            PftFor result = new PftFor(Tokens.Current);

            bool saveLoop = _inLoop;

            try
            {
                _inLoop = true;

                Tokens.RequireNext();
                PftTokenList initTokens = Tokens.Segment
                    (
                        _semicolonStop
                    )
                    .ThrowIfNull("initTokens");
                initTokens.Add(PftTokenKind.Semicolon);
                Tokens.Current.MustBe(PftTokenKind.Semicolon);
                ChangeContext(result.Initialization, initTokens);
                Tokens.RequireNext();
                PftTokenList conditionTokens = Tokens.Segment
                    (
                        _semicolonStop
                    )
                    .ThrowIfNull("conditionTokens");
                Tokens.Current.MustBe(PftTokenKind.Semicolon);
                result.Condition = (PftCondition)ChangeContext
                    (
                        conditionTokens,
                        ParseCondition
                    );
                Tokens.RequireNext();
                PftTokenList loopTokens = Tokens.Segment
                    (
                        _doStop
                    )
                    .ThrowIfNull("loopTokens");
                Tokens.Current.MustBe(PftTokenKind.Do);
                ChangeContext(result.Loop, loopTokens);
                Tokens.RequireNext();
                PftTokenList bodyTokens = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _loopStop
                    )
                    .ThrowIfNull("bodyTokens");
                Tokens.Current.MustBe(PftTokenKind.End);
                ChangeContext(result.Body, bodyTokens);
                Tokens.MoveNext();
            }
            finally
            {
                _inLoop = saveLoop;
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// ForEach loop.
        /// </summary>
        /// <example>
        /// foreach $x in v200^a,v200^e,'Hello'
        /// do
        ///     $x
        ///     #
        /// end
        /// </example>
        private PftNode ParseForEach()
        {
            PftForEach result = new PftForEach(Tokens.Current);

            bool saveLoop = _inLoop;

            try
            {
                _inLoop = true;

                Tokens.RequireNext();
                result.Variable = (PftVariableReference)ParseVariableReference();
                Tokens.Current.MustBe(PftTokenKind.In);
                Tokens.RequireNext();
                PftTokenList sequenceTokens = Tokens.Segment
                    (
                        _doStop
                    )
                    .ThrowIfNull("sequenceTokens");
                Tokens.Current.MustBe(PftTokenKind.Do);
                ChangeContext
                    (
                        result.Sequence,
                        sequenceTokens
                    );
                Tokens.RequireNext();
                PftTokenList bodyTokens = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _loopStop
                    )
                    .ThrowIfNull("bodyTokens");
                Tokens.Current.MustBe(PftTokenKind.End);
                ChangeContext
                    (
                        result.Body,
                        bodyTokens
                    );
                Tokens.MoveNext();
            }
            finally
            {
                _inLoop = saveLoop;
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// from ... select...
        /// </summary>
        /// <example>
        /// from $x in (v300/)
        /// where $x:'a'
        /// select 'Comment: ', $x
        /// order $x
        /// end
        /// </example>
        private PftNode ParseFrom()
        {
            PftFrom result = new PftFrom(Tokens.Current);

            bool saveLoop = _inLoop;

            try
            {
                _inLoop = true;

                // Variable reference
                Tokens.RequireNext(PftTokenKind.Variable);
                result.Variable = (PftVariableReference)ParseVariableReference();

                // In clause
                Tokens.Current.MustBe(PftTokenKind.In);
                Tokens.RequireNext();
                PftTokenList sourceTokens = Tokens.Segment(_whereStop)
                    .ThrowIfNull("sourceTokens");
                ChangeContext
                    (
                        result.Source,
                        sourceTokens
                    );

                // Where clause (optional)
                if (Tokens.Current.Kind == PftTokenKind.Where)
                {
                    Tokens.RequireNext();
                    PftTokenList whereTokens = Tokens.Segment(_selectStop)
                        .ThrowIfNull("whereTokens");
                    result.Where = (PftCondition)ChangeContext
                        (
                            whereTokens,
                            ParseCondition
                        );
                }

                // Select clause
                Tokens.Current.MustBe(PftTokenKind.Select);
                Tokens.RequireNext();
                PftTokenList selectTokens = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _orderStop
                    )
                    .ThrowIfNull("selectTokens");
                ChangeContext
                    (
                        result.Select,
                        selectTokens
                    );

                // Order clause (optional)
                if (Tokens.Current.Kind == PftTokenKind.Order)
                {
                    Tokens.RequireNext();
                    PftTokenList orderTokens = Tokens.Segment
                        (
                            _loopOpen,
                            _loopClose,
                            _loopStop
                        )
                        .ThrowIfNull("orderTokens");
                    ChangeContext
                        (
                            result.Order,
                            orderTokens
                        );
                }

                Tokens.Current.MustBe(PftTokenKind.End);
                Tokens.MoveNext();
            }
            finally
            {
                _inLoop = saveLoop;
            }

            return result;
        }

        //=================================================

        private void _HandleArguments
            (
                [NotNull] PftFunctionCall call,
                [NotNull] PftTokenList tokens
            )
        {
            PftTokenList saveTokens = Tokens;
            Tokens = tokens;

            try
            {
                PftNode superNode = new PftNode();
                while (!Tokens.IsEof)
                {
                    PftNode node = ParseNext();
                    superNode.Children.Add(node);
                }

                if (superNode.Children.Count == 1)
                {
                    superNode = superNode.Children[0];
                }

                call.Arguments.Add(superNode);
            }
            finally
            {
                Tokens = saveTokens;
            }
        }

        private PftNode ParseFunctionCall()
        {
            PftFunctionCall result = new PftFunctionCall(Tokens.Current);

            Tokens.RequireNext();

            PftToken token = Tokens.Current;
            token.MustBe(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();

            PftTokenList innerTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
                .ThrowIfNull("innerTokens");

            if (innerTokens.IsEof)
            {
                Tokens.MoveNext();
            }
            else
            {
                while (!innerTokens.IsEof)
                {
                    PftTokenList argumentTokens = innerTokens.Segment(_semicolonStop);
                    if (ReferenceEquals(argumentTokens, null))
                    {
                        _HandleArguments(result, innerTokens);
                        Tokens.MoveNext();
                        break;
                    }
                    _HandleArguments(result, argumentTokens);
                    innerTokens.MoveNext();
                }
            }

            return result;
        }

        //=================================================

        private PftNode ParseGroup()
        {
            PftGroup result = new PftGroup(Tokens.Current);

            if (_inGroup)
            {
                throw new PftSyntaxException("no nested group enabled");
            }

            try
            {
                _inGroup = true;

                ParseCall2(result);
            }
            finally
            {
                _inGroup = false;
            }

            return result;
        }

        //=================================================

        private PftNode ParseHash()
        {
            return MoveNext(new PftHash(Tokens.Current));
        }

        //=================================================

        private PftHave ParseHave()
        {
            PftHave result = new PftHave(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            if (Tokens.Current.Kind == PftTokenKind.Variable)
            {
                PftVariableReference variable
                    = (PftVariableReference)ParseVariableReference();
                result.Variable = variable;
            }
            else if (Tokens.Current.Kind == PftTokenKind.Identifier)
            {
                result.Identifier = Tokens.Current.Text;
                Tokens.MoveNext();
            }
            else
            {
                throw new PftSyntaxException(Tokens);
            }

            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            return MoveNext(result);

        }

        //=================================================

        private PftNode ParseL()
        {
            PftL result = new PftL(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseMfn()
        {
            return MoveNext(new PftMfn(Tokens.Current));
        }

        //=================================================

        private PftNode ParseMpl()
        {
            return MoveNext(new PftMode(Tokens.Current));
        }

        //=================================================

        private PftNode ParseNl()
        {
            return MoveNext(new PftNl(Tokens.Current));
        }

        //=================================================

        private PftNumeric ParseNumber()
        {
            return MoveNext(new PftNumericLiteral(Tokens.Current));
        }

        //=================================================

        private PftP ParseP()
        {
            PftP result = new PftP(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext(PftTokenKind.V);
            PftField field = (PftField)ParseField();
            if (ReferenceEquals(field, null))
            {
                throw new PftSyntaxException(Tokens.Current);
            }
            result.Field = field;
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            return MoveNext(result);
        }

        //=================================================

        private PftNode ParsePercent()
        {
            return MoveNext(new PftPercent(Tokens.Current));
        }

        //=================================================

        private PftNode ParseProc()
        {
            PftProcedureDefinition result
                = new PftProcedureDefinition(Tokens.Current);

            if (_inProcedure)
            {
                throw new PftSyntaxException("no nested proc allowed");
            }
            if (_inLoop)
            {
                throw new PftSyntaxException("no proc in loop allowed");
            }
            if (_inGroup)
            {
                throw new PftSyntaxException("no proc in group allowed");
            }

            try
            {
                _inProcedure = true;

                PftProcedure procedure = new PftProcedure();
                result.Procedure = procedure;

                Tokens.RequireNext(PftTokenKind.Identifier);
                procedure.Name = Tokens.Current.Text;
                Tokens.RequireNext(PftTokenKind.Do);
                Tokens.RequireNext();

                string name = procedure.Name
                    .ThrowIfNull("procedure.Name");
                if (name.OneOf(PftUtility.GetReservedWords()))
                {
                    throw new PftSyntaxException
                        (
                            "reserved word: " + name
                        );
                }
                if (PftFunctionManager.BuiltinFunctions.HaveFunction(name)
                   || PftFunctionManager.UserFunctions.HaveFunction(name)
                   )
                {
                    throw new PftSyntaxException
                        (
                            "already have function: " + name
                        );
                }
                if (!ReferenceEquals(_procedures.FindProcedure(name), null))
                {
                    throw new PftSyntaxException
                        (
                            "already have procedure: " + name
                        );
                }

                _procedures.Registry.Add
                (
                    name,
                    procedure
                );

                PftTokenList bodyList = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _procedureStop
                    )
                    .ThrowIfNull("bodyList");
                Tokens.Current.MustBe(PftTokenKind.End);
                ChangeContext
                (
                    procedure.Body,
                    bodyList
                );
                Tokens.MoveNext();

            }
            finally
            {
                _inProcedure = false;
            }

            return result;
        }

        //=================================================

        private PftNode ParseRef()
        {
            PftRef result = new PftRef(Tokens.Current);

            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            //result.Mfn = ParseNumber();
            result.Mfn = ParseArithmetic(PftTokenKind.Comma);
            Tokens.Current.MustBe(PftTokenKind.Comma);
            Tokens.RequireNext();
            PftNode pseudo = new PftNode();
            ParseCall3(pseudo);
            result.Format.AddRange(pseudo.Children);

            return result;
        }

        //=================================================

        private PftNode ParseRepeatableLiteral()
        {
            return MoveNext(new PftRepeatableLiteral(Tokens.Current));
        }

        //=================================================

        private PftNode ParseRsum()
        {
            PftNode result = new PftRsum(Tokens.Current);
            return ParseCall(result);
        }

        //=================================================

        private PftNode ParseS()
        {
            PftNode result = new PftS(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseSemicolon()
        {
            return MoveNext(new PftSemicolon(Tokens.Current));
        }

        //=================================================

        private PftNode ParseSlash()
        {
            return MoveNext(new PftSlash(Tokens.Current));
        }

        //=================================================

        private PftNode ParseUnconditionalLiteral()
        {
            return MoveNext(new PftUnconditionalLiteral(Tokens.Current));
        }

        //=================================================

        private PftNode ParseUnifor()
        {
            PftNode result = new PftUnifor(Tokens.Current);
            return ParseCall(result);
        }

        //=================================================

        private PftNode ParseVal()
        {
            PftNode result = new PftVal(Tokens.Current);
            return ParseCall(result);
        }

        //=================================================

        private PftNode ParseVariable()
        {
            PftToken firstToken = Tokens.Current;

            IndexSpecification index = ParseIndex();

            if (Tokens.Peek() == PftTokenKind.Equals)
            {
                if (_inAssignment)
                {
                    throw new PftSyntaxException("nested assignment");
                }

                try
                {
                    _inAssignment = true;

                    PftAssignment result = new PftAssignment(firstToken)
                    {
                        Index = index
                    };
                    Tokens.RequireNext(PftTokenKind.Equals);
                    Tokens.RequireNext();

                    PftTokenList tokens = Tokens.Segment
                        (
                            _semicolonStop
                        );
                    if (ReferenceEquals(tokens, null))
                    {
                        throw new PftSyntaxException(Tokens);
                    }
                    Tokens.Current.MustBe(PftTokenKind.Semicolon);
                    Tokens.MoveNext();

                    result = ParseAssignment(result, tokens);

                    return result;
                }
                finally
                {
                    _inAssignment = false;
                }
            }

            PftNode reference = new PftVariableReference(firstToken)
            {
                Index = index,
                SubFieldCode = ParseSubField()
            };

            return MoveNext(reference);
        }

        //=================================================

        private PftNode ParseVariableReference()
        {
            PftVariableReference result
                = new PftVariableReference(Tokens.Current)
            {
                Index = ParseIndex(),
                SubFieldCode = ParseSubField()
            };

            return MoveNext(result);
        }

        //=================================================

        private PftNode ParseVerbatim()
        {
            return MoveNext(new PftVerbatim(Tokens.Current));
        }

        //=================================================

        /// <summary>
        /// While loop.
        /// </summary>
        /// <example>
        /// $x=0;
        /// while $x &lt; 10
        /// do
        ///     $x, ') ',
        ///     'Прикольно же!'
        ///     #
        ///     $x=$x+1;
        /// end
        /// </example>
        private PftNode ParseWhile()
        {
            PftWhile result = new PftWhile(Tokens.Current);

            bool saveLoop = _inLoop;

            try
            {
                _inLoop = true;

                Tokens.RequireNext();
                PftTokenList conditionTokens = Tokens.Segment
                    (
                        _doStop
                    )
                    .ThrowIfNull("conditionTokens");
                Tokens.Current.MustBe(PftTokenKind.Do);
                result.Condition = (PftCondition)ChangeContext
                    (
                        conditionTokens,
                        ParseCondition
                    );
                Tokens.RequireNext();
                PftTokenList bodyTokens = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _loopStop
                    )
                    .ThrowIfNull("bodyTokens");
                Tokens.Current.MustBe(PftTokenKind.End);
                ChangeContext
                    (
                        result.Body,
                        bodyTokens
                    );
                Tokens.MoveNext();
            }
            finally
            {
                _inLoop = saveLoop;
            }

            return result;
        }

        //=================================================

        private PftNode ParseX()
        {
            return MoveNext(new PftX(Tokens.Current));
        }

        //================================================================
        // Other routines
        //================================================================

        private PftProgram ParseProgram()
        {
            PftProgram result = new PftProgram();

            while (!Tokens.IsEof)
            {
                PftNode node = ParseNext();
                result.Children.Add(node);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the tokens.
        /// </summary>
        [NotNull]
        public PftProgram Parse()
        {
            PftProgram result = ParseProgram();
            result.Procedures = _procedures;

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
