using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;

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
        Spinner _spinner;

        private Transport _transport;

        bool _busy;

        BookRequest[] _activeRequests;
        BookRequest _currentRequest;
        int _currentIndex;

        protected override void OnCreate (Bundle bundle)
        {
            try
            {
                base.OnCreate (bundle);

                SetContentView (Resource.Layout.activity_main);
                _transport = Transport.Load(this);

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
                _spinner = FindViewById<Spinner>(Resource.Id.inventorySpinner);

                _rangeText.Text = $"фонд: {_transport.Place}, номера: {_transport.My}";

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
                _orderView.Text = $"IP: {_transport.Host}, port:{_transport.Port}, login: {_transport.Login}\n\n"
                    + ex;

                return;
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

            try
            {
                _busy = true;

                using (RequestManager reqMan = new RequestManager (_transport))
                {
                    _activeRequests = reqMan.GetRequests ();

                    foreach (BookRequest request in _activeRequests)
                    {
                        reqMan.GetAdditionalInfo (request);
                    }

                    _activeRequests = _activeRequests
                        .Where(r => !r.MyNumbers.IsNullOrEmpty())
                        .ToArray();

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

            int length = _activeRequests?.Length ?? 0;
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

                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this,
                    Android.Resource.Layout.SimpleSpinnerItem, Array.Empty<string>());
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                _spinner.Adapter = adapter;
                _spinner.Prompt = "Выберите экземпляр";
            }
            else
            {
                SetDetails (_currentRequest.ToString ());

                string[] numbers = new string[0];
                if (!_currentRequest.MyNumbers.IsNullOrEmpty())
                {
                    numbers = _currentRequest.MyNumbers;
                }

                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this,
                    Android.Resource.Layout.SimpleSpinnerItem, numbers);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                _spinner.Adapter = adapter;
                _spinner.Prompt = "Выберите экземпляр";
                if (numbers.Length > 0)
                {
                    _spinner.SetSelection(0);
                }
            }
        }

        void SetDone ()
        {
            MarcRecord requestRecord = _currentRequest?.Record;
            if (ReferenceEquals(requestRecord, null))
            {
                return;
            }

            MarcRecord bookRecord = _currentRequest.BookRecord;
            if (ReferenceEquals(bookRecord, null))
            {
                SetDetails("Нет записи о книге");
                return;
            }

            string myNumber = _spinner?.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(myNumber))
            {
                SetDetails("Нет доступных экземпляров");
                return;
            }

            RecordField field = bookRecord.Fields
                .GetField(910)
                .FirstOrDefault(f => f.GetFirstSubFieldValue('b')
                    .SameString(myNumber));
            if (ReferenceEquals(field, null))
            {
                SetDetails("Нет поля для экземпляра");
                return;
            }

            // Сведения о забронированном экземпляре
            RecordField copy = field.Clone().SetSubField('a', "0");
            requestRecord.Fields.Add(copy);

            // Дата предполагаемого возврата
            requestRecord.AddField (42, DateTime.Now.AddDays(14).ToString("MM-dd-yyyy  hh:mm:ss"));

            // Дата бронирования
            requestRecord.AddField(43, DateTime.Now.ToString("MM-dd-yyyy  hh:mm:ss"));

            // Ответственное лицо
            requestRecord.AddField(50, "kladovka");

            // Статус экземпляра: забронировано
            field.SetSubField('a', "9");

            using (RequestManager reqMan = new RequestManager(_transport))
            {
                reqMan.WriteRequest (_currentRequest);
            }
        }

        void SetReject
            (
                string value
            )
        {
            MarcRecord record = _currentRequest?.Record;
            if (ReferenceEquals(record, null))
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
            using (RequestManager reqMan = new RequestManager (_transport))
            {
                reqMan.WriteRequest (_currentRequest);
            }
        }

        void _OnPrevButtonClick (object sender, EventArgs ea)
        {
            try
            {
                BookRequest request = _currentRequest;
                if (ReferenceEquals(request, null))
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
                BookRequest request = _currentRequest;
                if (ReferenceEquals(request, null))
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
                if (ReferenceEquals(request, null))
                {
                    return;
                }

                await Task.Run(() => SetDone ());
                await UpdateStateAsync ();
                _updateText.Text = $"Заказ был выполнен ({DateTime.Now})";

                // Не нужно отправлять письмо
                // BookRequest request = _currentRequest;
                // await SendMailAsync(request, false);
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
                if (ReferenceEquals(request, null))
                {
                    return;
                }

                await Task.Run(() => SetReject ("01"));
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

            builder.Append($"{request.ReaderDescription}<br/>");
            builder.Append($"<i>Заказ {request.RequestDate}</i><br/>");
            builder.Append("<br/>");
            builder.Append($"{request.BookDescription}<br/>");
            if (rejection)
            {
                builder.Append("<br/>");
                builder.Append("К сожалению в настоящее время Ваш заказ "
                + "не может быть выполнен. За дополнительной информацией "
                + "Вы можете обратиться в справочно-библиграфический отдел "
                + "библиотеки: spravka@irklib.ru, либо по телефону "
                + "+7 (3952) 48-66-80 доп. 570<br/>");
            }
            else
            {
                builder.Append($"Заказ выполнен и ожидает: {request.Place}<br/>");
            }

            string mailBody = builder.ToString();

            _updateText.Text = "ОТПРАВКА ПИСЬМА";

            try
            {
                await _transport.SendMail(request, mailBody);

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


