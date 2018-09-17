// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UpdateIniFileCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using AM;
using AM.IO;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class UpdateIniFileCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UpdateIniFileCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region Private members

        [CanBeNull]
        private IniFile _GetUserIniFile()
        {
            IrbisServerEngine engine = Data.Engine.ThrowIfNull();
            UserInfo user = Data.User.ThrowIfNull();
            string workstation = Data.Request.Workstation.ThrowIfNull();
            string filename;
            switch (workstation)
            {
                case "a":
                case "A":
                    filename = engine.GetDefaultIniName(user.Administrator, 5);
                    break;

                case "b":
                case "B":
                    filename = engine.GetDefaultIniName(user.Circulation, 2);
                    break;

                case "c":
                case "C":
                    filename = engine.GetDefaultIniName(user.Cataloger, 0);
                    break;

                case "k":
                case "K":
                    filename = engine.GetDefaultIniName(user.Provision, 4);
                    break;

                case "m":
                case "M":
                    filename = engine.GetDefaultIniName(user.Cataloger, 0);
                    break;

                case "r":
                case "R":
                    filename = engine.GetDefaultIniName(user.Reader, 1);
                    break;

                case "p":
                case "P":
                    filename = engine.GetDefaultIniName(user.Acquisitions, 3);
                    break;

                default:
                    // Недопустимый клиент
                    return null;
            }

            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            try
            {
                string ext = Path.GetExtension(filename);
                if (string.IsNullOrEmpty(ext))
                {
                    filename = filename + ".ini";
                }
                filename = Path.Combine(engine.SystemPath, filename);
                IniFile result = new IniFile(filename, IrbisEncoding.Ansi, true);

                return result;
            }
            catch(Exception exception)
            {
                Log.TraceException("IrbisServerEngine::GetUserIniFile", exception);
                return null;
            }
        }

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            IrbisServerEngine engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute(Data);

            try
            {
                ServerContext context = engine.RequireContext(Data);
                Data.Context = context;
                UpdateContext();

                ClientRequest request = Data.Request.ThrowIfNull();
                Data.User = engine.FindUser(request.Login);
                IniFile iniFile = _GetUserIniFile();
                if (!ReferenceEquals(iniFile, null))
                {
                    string text = request.RemainingAnsiText();
                    TextReader reader = new StringReader(text);
                    IniFile patch = new IniFile();
                    patch.Read(reader);

                    foreach (IniFile.Section section in patch.GetSections())
                    {
                        iniFile.UpdateSection(section);
                    }

                    try
                    {
                        iniFile.Save(iniFile.FileName);
                    }
                    catch (Exception ex)
                    {
                        Log.TraceException("UpdateIniFileCommand::Execute", ex);
                    }
                }

                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteInt32(0).NewLine();
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("UpdateIniFileCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
