using System;
using System.IO;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Text.Method;
using ManagedIrbis;

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
        private string _requestDB;
        private string _catalogDB;
        private string _readerDB;
        private string _place;
        private string _my;

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
                    = new StreamReader (Assets.Open ("settings.txt"))) {
                    _host = reader.ReadLine ();
                    _port = int.Parse (reader.ReadLine ());
                    _login = reader.ReadLine ();
                    _password = reader.ReadLine ();
                    _requestDB = reader.ReadLine ();
                    _catalogDB = reader.ReadLine ();
                    _readerDB = reader.ReadLine ();
                    _place = reader.ReadLine ();
                    _my = reader.ReadLine ();
                    _auto = int.Parse (reader.ReadLine ());
                    _sound = Convert.ToBoolean (int.Parse (reader.ReadLine ()));
                }

                _rangeText.Text = string.Format ("фонд: {0}, номера: {1}", _place, _my);

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

        void UpdateState ()
        {
            if (_busy) {
                return;
            }

            _activeRequests = null;
            _currentRequest = null;

            try {
                _busy = true;

                using (RequestManager reqMan = new RequestManager (this)) {
                    _activeRequests = reqMan.GetRequests ();

                    foreach (BookRequest request in _activeRequests) {
                        reqMan.GetAdditionalInfo (request);
                    }

                    _activeRequests = _activeRequests
                    .Where (r => r.MyNumbers.Length != 0)
                    .ToArray ();

                    if (_activeRequests.Length <= _currentIndex) {
                        _currentIndex = _activeRequests.Length - 1;
                    }
                    if (_currentIndex < 0) {
                        _currentIndex = 0;
                    }

                    if ((_activeRequests.Length != 0)
                        && (_currentIndex >= 0)
                        && (_currentIndex < _activeRequests.Length)) {
                        _currentRequest = _activeRequests [_currentIndex];
                    }

                    NavigateRequest (_currentIndex, true);
                }

                _updateText.Text = string.Format
                    (
                    "Заказов: {0} ({1})",
                    _activeRequests.Length,
                    DateTime.Now
                );
            } catch (Exception ex) {
                _statusText.Text = "Ошибка";
                _updateText.Text = "Произошло исключение";
                _orderView.Text = ex.ToString ();
            } finally {
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
            if (_activeRequests == null) {
                _currentIndex = 0;
                _currentRequest = null;
            } else {
                if (_currentIndex >= _activeRequests.Length) {
                    _currentIndex = _activeRequests.Length - 1;
                }
                if (_currentIndex < 0) {
                    _currentIndex = 0;
                }
                _currentRequest = (_currentIndex >= _activeRequests.Length)
                    ? null
                    : _activeRequests [_currentIndex];
            }
            int length = (_activeRequests == null)
                ? 0
                : _activeRequests.Length;
            if (length == 0) {
                SetStatus ("Нет заказов");
            } else {
                SetStatus ("Заказ {0} из {1}", _currentIndex + 1, length);
            }
            if (_currentRequest == null) {
                SetDetails ("В настоящее время заказов нет\n\n"
                + "Через некоторое время нажмите кнопку ОБНОВИТЬ снова.");
            } else {
                SetDetails (_currentRequest.ToString ());
            }
        }

        void SetDone ()
        {
            if (_currentRequest == null) {
                return;
            }

            MarcRecord record = _currentRequest.Record;
            if (record == null) {
                return;
            }

            record.AddField (41, DateTime.Now.ToString ("dd-MM-yyyy hh:mm:ss"));
            record.AddField (42, Today ());
            using (RequestManager reqMan = new RequestManager(this)) {
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
            if (record == null) {
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
            try {
                if (_activeRequests == null) {
                    return;
                }
                int newIndex = _currentIndex - 1;
                if ((newIndex < 0) && (_activeRequests.Length > 0)) {
                    newIndex = _activeRequests.Length - 1;
                }
                NavigateRequest (newIndex, false);
            } catch (Exception ex) {
                _orderView.Text = ex.ToString ();
            }
        }

        void _OnNextButtonClick (object sender, EventArgs ea)
        {
            try {
                if (_activeRequests == null) {
                    return;
                }
                int newIndex = _currentIndex + 1;
                if ((_activeRequests.Length > 0) && (newIndex >= _activeRequests.Length)) {
                    newIndex = 0;
                }
                NavigateRequest (newIndex, false);
            } catch (Exception ex) {
                _orderView.Text = ex.ToString ();
            }
        }

        void _OnAcceptButtonClick (object sender, EventArgs ea)
        {
            try {
                SetDone ();
                UpdateState ();
                _updateText.Text = string.Format ("Заказ был выполнен ({0})",
                    DateTime.Now);
            } catch (Exception ex) {
                _orderView.Text = ex.ToString ();
            }
        }

        void _OnRejectButtonClick (object sender, EventArgs ea)
        {
            try {
                SetReject ("01");
                UpdateState ();
                _updateText.Text = string.Format ("Заказ был отменен ({0})",
                    DateTime.Now);
            } catch (Exception ex) {
                _orderView.Text = ex.ToString ();
            }
        }

        void _OnUpdateButtonClick (object sender, EventArgs ea)
        {
            UpdateState ();
        }
    }
}


