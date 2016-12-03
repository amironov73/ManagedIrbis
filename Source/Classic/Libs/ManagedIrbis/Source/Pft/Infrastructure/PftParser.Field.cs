// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParser.Field.cs --
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    using Ast;

    partial class PftParser
    {
        private PftField ParseField()
        {
            List<PftNode> leftHand = new List<PftNode>();
            PftField result;
            PftNode node;
            PftRepeatableLiteral literal;
            PftToken token;

            // Gather left hand of the field: conditional literal and friends
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;
                if (token.Kind == PftTokenKind.RepeatableLiteral
                    || token.Kind == PftTokenKind.V)
                {
                    break;
                }

                node = Get(FieldMap, LeftHandItems1);
                if (!ReferenceEquals(node, null))
                {
                    leftHand.Add(node);
                }
            } // Tokens.IsEof

            // Gather left hand of the field: repeatable literal
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;
                if (token.Kind == PftTokenKind.RepeatableLiteral)
                {
                    literal = new PftRepeatableLiteral(token)
                    {
                        IsPrefix = true
                    };
                    leftHand.Add(literal);

                    if (Tokens.Peek() == PftTokenKind.Plus)
                    {
                        literal.Plus = true;
                        Tokens.MoveNext();
                    }

                    Tokens.MoveNext();
                }
                else
                {
                    node = Get(FieldMap, LeftHandItems2);
                    if (ReferenceEquals(node, null))
                    {
                        break;
                    }
                    leftHand.Add(node);
                }
            } // Tokens.IsEof

            // Orphaned left hand?
            if (Tokens.IsEof)
            {
                result = new PftOrphan();
                result.LeftHand.AddRange(leftHand);
                goto DONE;
            }

            //
            // Parse field itself
            //

            token = Tokens.Current;
            //PftToken leadToken = token;

            // Orphaned?
            if (token.Kind != PftTokenKind.V)
            {
                result = new PftOrphan();
                result.LeftHand.AddRange(leftHand);
                goto DONE;
            }
            if (string.IsNullOrEmpty(token.Text))
            {
                throw new PftSyntaxException(token);
            }

            FieldSpecification specification
                = (FieldSpecification)token.UserData;
            if (ReferenceEquals(specification, null))
            {
                throw new PftSyntaxException(token);
            }

            // Check for command code
            switch (specification.Command)
            {
                case 'v':
                case 'V':
                    result = new PftV(token);
                    break;

                case 'd':
                case 'D':
                    result = new PftD(token);
                    break;

                case 'n':
                case 'N':
                    result = new PftN(token);
                    break;

                case 'g':
                case 'G':
                    result = new PftG(token);
                    break;

                default:
                    throw new PftSyntaxException(token);
            }

            result.LeftHand.AddRange(leftHand);
            result.Apply(specification);
            Tokens.MoveNext();

            // Gather right hand (for V command only)
            if (result is PftV)
            {
                if (!Tokens.IsEof)
                {
                    bool plus = false;
                    token = Tokens.Current;
                    if (token.Kind == PftTokenKind.Plus)
                    {
                        plus = true;
                        Tokens.RequireNext();
                        token = Tokens.Current;
                    }
                    if (token.Kind == PftTokenKind.RepeatableLiteral)
                    {
                        literal = new PftRepeatableLiteral(token)
                        {
                            Plus = plus
                        };
                        result.RightHand.Add(literal);
                        Tokens.MoveNext();
                    }
                    else
                    {
                        if (plus)
                        {
                            throw new PftSyntaxException(token);
                        }

                        node = Get(FieldMap, RightHandItems);
                        if (!ReferenceEquals(node, null))
                        {
                            result.RightHand.Add(node);
                        }
                    }
                } // Tokens.IsEof

                if (!Tokens.IsEof)
                {
                    token = Tokens.Current;
                    if (token.Kind == PftTokenKind.ConditionalLiteral)
                    {
                        node = new PftConditionalLiteral(token);
                        result.RightHand.Add(node);
                        Tokens.MoveNext();
                    }
                }
            } // result is PftV

            DONE: return result;
        }

        private PftNode ParseFieldAssignment()
        {
            PftToken headToken = Tokens.Current;
            PftField field = ParseField();
            PftNode result = field;

            if (!Tokens.IsEof
                && Tokens.Current.Kind == PftTokenKind.Equals)
            {
                Tokens.RequireNext();

                PftFieldAssignment assignment = new PftFieldAssignment(headToken)
                {
                    Field = field
                };

                PftTokenList tokens = Tokens.Segment(_semicolonStop)
                    .ThrowIfNull("tokens");
                ChangeContext
                    (
                        (NonNullCollection<PftNode>)assignment.Children,
                        tokens
                    );

                Tokens.MoveNext();

                result = assignment;
            }

            return result;
        }
    }
}
