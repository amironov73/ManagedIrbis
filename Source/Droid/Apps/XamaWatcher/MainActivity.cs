using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AM;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Text.Method;

using ManagedIrbis;
using ManagedIrbis.Readers;

using MimeKit;

using SmtpClient = MailKit.Net.Smtp.SmtpClient;

// ReSharper disable StringLiteralTypo

namespace XamaWatcher
{
    [Activity (Label = "Мониторинг заказов", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button _prevButton;
        Button _nextButton;
        Button _acceptButton;
        Button _rejectButton;
        Button _updateButton;
        TextView _rangeText;
        TextView _statusText;
        TextView _updateText;
        TextView _orderView;

        private string _host;
        private string _login;
        private string _password;
        private string _requestDb;
        private string _catalogDb;
        private string _readerDb;
        private string _place;
        private string _my;
        private string _mailLogin;
        private string _mailPassword;
        private string _mailFrom;
        private string _mailSubject;
        private string _mailServer;

        int _port, _auto;
        bool _sound;

        bool _busy;

        BookRequest[] _activeRequests;
        BookRequest _currentRequest;
        int _currentIndex;

        protected override void OnCreate (Bundle bundle)
        {
            try
            {
                base.OnCreate (bundle);

                // Set our view from the "main" layout resource
                SetContentView (Resource.Layout.activity_main);

                _prevButton = FindViewById<Button> (Resource.Id.prevButton);
                _prevButton.Click += _OnPrevButtonClick;
                _nextButton = FindViewById<Button> (Resource.Id.nextButton);
                _nextButton.Click += _OnNextButtonClick;
                _acceptButton = FindViewById<Button> (Resource.Id.acceptButton);
                _acceptButton.Click += _OnAcceptButtonClick;
                _rejectButton = FindViewById<Button> (Resource.Id.rejectButton);
                _rejectButton.Click += _OnRejectButtonClick;
                _updateButton = FindViewById<Button> (Resource.Id.updateButton);
                _updateButton.Click += _OnUpdateButtonClick;
                _rangeText = FindViewById<TextView> (Resource.Id.rangeText);
                _statusText = FindViewById<TextView> (Resource.Id.statusText);
                _updateText = FindViewById<TextView> (Resource.Id.updateText);
                _orderView = FindViewById<TextView> (Resource.Id.orderText);
                _orderView.MovementMethod = new ScrollingMovementMethod();

                using (StreamReader reader
                    = new StreamReader (Assets.Open ("settings.txt")))
                {
                    _host = reader.ReadLine ();
                    _port = int.Parse (reader.ReadLine ());
                    _login = reader.ReadLine ();
                    _password = reader.ReadLine ();
                    _requestDb = reader.ReadLine ();
                    _catalogDb = reader.ReadLine ();
                    _readerDb = reader.ReadLine ();
                    _place = reader.ReadLine ();
                    _my = reader.ReadLine ();
                    _auto = int.Parse (reader.ReadLine ());
                    _sound = Convert.ToBoolean (int.Parse (reader.ReadLine ()));
                    _mailLogin = reader.ReadLine();
                    _mailPassword = reader.ReadLine();
                    _mailFrom = reader.ReadLine();
                    _mailSubject = reader.ReadLine();
                    _mailServer = reader.ReadLine();
                }

                _rangeText.Text = $"фонд: {_place}, номера: {_my}";

            }
            catch (Exception ex)
            {
                new AlertDialog.Builder (this)
                    .SetTitle ("Возникло исключение")
                    .SetMessage (ex.ToString ())
                    .SetPositiveButton ("ОК", (s, e) => {
                })
                    .Show ();
            }
        }

        void SetStatus (string format, params object[] args)
        {
            _statusText.Text = string.Format (format, args);
        }

        void SetDetails (string text)
        {
            _orderView.Text = text;
        }

        void SetUpdate (string format, params object[] args)
        {
            _updateText.Text = string.Format (format, args);
        }

        string Today ()
        {
            return DateTime.Today.ToString ("yyyyMMdd");
        }

        async Task UpdateStateAsync()
        {
            _updateText.Text = "ОБРАЩЕНИЕ К СЕРВЕРУ";

            try
            {
                await Task.Run(() => { UpdateState(); });
            }
            catch (Exception ex)
            {
                _statusText.Text = "Ошибка";
                _updateText.Text = "Произошло исключение";
                _orderView.Text = ex.ToString ();
            }

            NavigateRequest (_currentIndex, true);

            if (_activeRequests != null)
            {
                _updateText.Text = $"Заказов: {_activeRequests.Length} ({DateTime.Now})";
            }
            else
            {
                _updateText.Text = string.Empty;
            }
        }

        void UpdateState ()
        {
            if (_busy)
            {
                return;
            }

            _activeRequests = null;
            _currentRequest = null;

            //ProgressDialog progress = new ProgressDialog(this)
            //{
            //    Indeterminate = true
            //};
            //progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            //progress.SetMessage("Подождите...");
            //progress.SetCancelable(false);
            //progress.Show();

            try
            {
                _busy = true;

                using (RequestManager reqMan = new RequestManager (this))
                {
                    _activeRequests = reqMan.GetRequests ();

                    foreach (BookRequest request in _activeRequests)
                    {
                        reqMan.GetAdditionalInfo (request);
                    }

                    _activeRequests = _activeRequests
                        .Where (r => r.MyNumbers.Length != 0)
                        .ToArray ();

                    if (_activeRequests.Length <= _currentIndex)
                    {
                        _currentIndex = _activeRequests.Length - 1;
                    }

                    if (_currentIndex < 0) {
                        _currentIndex = 0;
                    }

                    if (_activeRequests.Length != 0
                        && _currentIndex >= 0
                        && _currentIndex < _activeRequests.Length)
                    {
                        _currentRequest = _activeRequests [_currentIndex];
                    }
                }
            }
            finally
            {
                //progress.Dismiss();
                _busy = false;
            }
        }

        void NavigateRequest
            (
                int index,
                bool sound
            )
        {
            _currentIndex = index;
            if (_activeRequests == null)
            {
                _currentIndex = 0;
                _currentRequest = null;
            }
            else
            {
                if (_currentIndex >= _activeRequests.Length) {
                    _currentIndex = _activeRequests.Length - 1;
                }
                if (_currentIndex < 0) {
                    _currentIndex = 0;
                }
                _currentRequest = _currentIndex >= _activeRequests.Length
                    ? null
                    : _activeRequests [_currentIndex];
            }

            int length = _activeRequests == null
                ? 0
                : _activeRequests.Length;
            if (length == 0)
            {
                SetStatus ("Нет заказов");
            }
            else
            {
                SetStatus ("Заказ {0} из {1}", _currentIndex + 1, length);
            }
            if (_currentRequest == null)
            {
                SetDetails ("В настоящее время заказов нет\n\n"
                + "Через некоторое время нажмите кнопку ОБНОВИТЬ снова.");
            }
            else
            {
                SetDetails (_currentRequest.ToString ());
            }
        }

        void SetDone ()
        {
            if (_currentRequest == null)
            {
                return;
            }

            MarcRecord record = _currentRequest.Record;
            if (record == null)
            {
                return;
            }

            record.AddField (41, DateTime.Now.ToString ("dd-MM-yyyy hh:mm:ss"));
            record.AddField (42, Today ());
            using (RequestManager reqMan = new RequestManager(this))
            {
                reqMan.WriteRequest (_currentRequest);
            }
        }

        void SetReject (string value)
        {
            if (_currentRequest == null)
            {
                return;
            }

            MarcRecord record = _currentRequest.Record;
            if (record == null)
            {
                return;
            }

            record.AddField
                (
                    new RecordField
                    (
                        44,
                        new SubField('a', value),
                        new SubField('b', Today())
                    )
                );
            using (RequestManager reqMan = new RequestManager (this))
            {
                reqMan.WriteRequest (_currentRequest);
            }
        }

        void _OnPrevButtonClick (object sender, EventArgs ea)
        {
            try
            {
                if (_activeRequests == null)
                {
                    return;
                }

                int newIndex = _currentIndex - 1;
                if (newIndex < 0 && _activeRequests.Length > 0)
                {
                    newIndex = _activeRequests.Length - 1;
                }
                NavigateRequest (newIndex, false);
            }
            catch (Exception ex)
            {
                _orderView.Text = ex.ToString ();
            }
        }

        void _OnNextButtonClick (object sender, EventArgs ea)
        {
            try
            {
                if (_activeRequests == null)
                {
                    return;
                }

                int newIndex = _currentIndex + 1;
                if (_activeRequests.Length > 0 && newIndex >= _activeRequests.Length)
                {
                    newIndex = 0;
                }
                NavigateRequest (newIndex, false);
            }
            catch (Exception ex)
            {
                _orderView.Text = ex.ToString ();
            }
        }

        async void _OnAcceptButtonClick
            (
                object sender,
                EventArgs ea
            )
        {
            try
            {
                BookRequest request = _currentRequest;
                SetDone ();
                await UpdateStateAsync ();
                _updateText.Text = $"Заказ был выполнен ({DateTime.Now})";
                await SendMailAsync(request, false);
            }
            catch (Exception ex)
            {
                _orderView.Text = ex.ToString ();
            }
        }

        async void _OnRejectButtonClick
            (
                object sender,
                EventArgs ea
            )
        {
            try
            {
                BookRequest request = _currentRequest;
                SetReject ("01");
                await UpdateStateAsync();
                _updateText.Text = $"Заказ был отменен ({DateTime.Now})";
                await SendMailAsync(request, true);
            }
            catch (Exception ex)
            {
                _orderView.Text = ex.ToString ();
            }
        }

        async void _OnUpdateButtonClick
            (
                object sender,
                EventArgs ea
            )
        {
            // UpdateState ();
            await UpdateStateAsync();
        }

        async Task SendMailAsync
            (
                BookRequest request,
                bool rejection
            )
        {
            if (ReferenceEquals(request, null)
               || ReferenceEquals(request.Reader, null)
               || string.IsNullOrEmpty(request.Reader.Email))
            {
                return;
            }

            StringBuilder builder = new StringBuilder();

            //string appeal = "Уважаемый";
            //ReaderInfo reader = _currentRequest.Reader;
            //if (!ReferenceEquals(appeal, null))
            //{
            //    Gender gender = GenderUtility.Parse(reader.Gender);
            //    if (gender == Gender.Female)
            //    {
            //        appeal = "Уважаемая";
            //    }
            //}

            builder.Append($"{request.ReaderDescription}<br/>");
            builder.Append($"Заказ {request.RequestDate}<br/>");
            builder.Append($"{request.BookDescription}<br/>");
            if (rejection)
            {
                builder.Append("К сожалению, заказ не может быть выполнен<br/>");
            }
            else
            {
                builder.Append($"Заказ выполнен и ожидает: {request.Place}<br/>");
            }

            string mailBody = builder.ToString();
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailFrom, _mailLogin));
            message.To.Add(new MailboxAddress(request.Reader.FullName, request.Reader.Email));
            message.Subject = _mailSubject;
            message.Body = new TextPart("html")
            {
                Text = mailBody
            };

            _updateText.Text = "ОТПРАВКА ПИСЬМА";

            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(_mailServer, 587, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_mailLogin, _mailPassword);
                    await client.SendAsync(message);
                }

                _updateText.Text = "ПИСЬМО ОТПРАВЛЕНО";
            }
            catch (Exception ex)
            {
                _statusText.Text = "ОШИБКА";
                _updateText.Text = "Произошло исключение";
                _orderView.Text = ex.ToString();
            }
        }
    }
}


