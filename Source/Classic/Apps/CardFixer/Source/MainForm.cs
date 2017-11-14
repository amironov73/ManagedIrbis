using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Text.Output;

using ManagedIrbis;

namespace CardFixer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private DataAccessLevel _dal;

        // ReSharper disable once InconsistentNaming
        private AbstractOutput _output { get; set; }

        private Card _currentCard;

        // ReSharper disable once MemberCanBePrivate.Global
        public Card CurrentCard
        {
            get { return _currentCard; }
            set { _SetCurrentCard(value); }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                FileOutput fileOutput = new FileOutput("CardFixer.log");
                AbstractOutput output = new TeeOutput(_logBox.Output, fileOutput);
                _output = output;
                _output.WriteLine(new string('=', 70));
                _output.WriteLine
                    (
                        "{0}: ЗАПУСК ПРОГРАММЫ",
                        DateTime.Now
                    );

                _AddVersionToTitle();

                _dal = new DataAccessLevel(_output);

                _output.WriteLine
                    (
                        "Всего карточек: {0}",
                        _dal.GetTotalCardCount()
                    );
                _output.WriteLine
                    (
                        "Списанных номеров: {0}",
                        _dal.GetBadNumberCount()
                    );
                _output.WriteLine
                    (
                        "Карточек в ИРБИС: {0}",
                        _dal.GetIrbisCardCount()
                    );

                int[] numbers = _dal.GetBoxNumbers(CardStatus.AlreadyHave);
                _boxNumberBox.Items.Clear();
                _boxNumberBox.Items.AddRange
                    (
                        numbers
                            .Select(i => (object)i)
                            .ToArray()
                    );
                if (numbers.Length != 0)
                {
                    _boxNumberBox.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show
                    (
                        ex.Message,
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                Environment.FailFast("Ошибка");
            }
        }

        private bool _busy;

        private void _AddVersionToTitle()
        {
            Assembly assembly
                = Assembly.GetExecutingAssembly();
            Version vi = assembly.GetName().Version;
            FileVersionInfo fvi = FileVersionInfo
                .GetVersionInfo(assembly.Location);
            FileInfo fi = new FileInfo(assembly.Location);
            Text += string.Format
                (
                    ": версия {0} (файл {1}) от {2}",
                    vi,
                    fvi.FileVersion,
                    fi.LastWriteTime.ToShortDateString()
                );
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!ReferenceEquals(_output, null))
            {
                _output.WriteLine
                    (
                        "{0}: ЗАВЕРШЕНИЕ ПРОГРАММЫ",
                        DateTime.Now
                    );
                _output.WriteLine(new string('=', 70));
                _output.Dispose();
                _output = AbstractOutput.Null;
            }
            if (!ReferenceEquals(_dal, null))
            {
                _dal.Dispose();
            }
        }

        private void _SetCurrentCard(Card card)
        {
            //ClearInventories();
            _pictureBox.Image = null;
            _currentCard = card;
            if (card == null)
            {
                return;
            }
            card.FullPath = Card.CalculateFullPath(card);
            //Login = card.Operator;
            //Remark = card.Remark;
            Image image = Image.FromFile(card.FullPath);
            _pictureBox.Image = image;
        }

        private void _boxNumberBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listBox.Items.Clear();
            _pictureBox.Image = null;
            int index = _boxNumberBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            object o = _boxNumberBox.SelectedItem;
            // ReSharper disable once MergeSequentialChecks
            if ((o == null)
                || !(o is int))
            {
                return;
            }
            int boxNumber = (int)o;
            CardStatus status = CardStatus.AlreadyHave;
            Card[] cards = _dal.GetCards
                (
                    boxNumber,
                    status
                );
            // ReSharper disable once CoVariantArrayConversion
            _listBox.Items.AddRange
                (
                    cards
                );
            _output.WriteLine
                (
                    "Получили описания {0} карточек "
                    + "из ящика {1}",
                    cards.Length,
                    boxNumber
                );
            if (cards.Length != 0)
            {
                _listBox.SelectedIndex = 0;
            }
        }

        private void _listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentCard = null;
            _pictureBox.Image = null;
            int index = _listBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            object o = _listBox.SelectedItem;
            // ReSharper disable once MergeSequentialChecks
            if ((o == null)
                || !(o is Card))
            {
                return;
            }
            CurrentCard = (Card)o;
        }

        public void CheckInventory(string inventory)
        {
            if (string.IsNullOrEmpty(inventory))
            {
                MoveNext();
                return;
            }
            inventory = inventory.Trim();
            if (string.IsNullOrEmpty(inventory))
            {
                MoveNext();
                return;
            }

            InventoryResult result = _dal.CheckInventory2(inventory);
            if (result.Status != InventoryStatus.Found)
            {
                _output.WriteLine("Не найдена карточка для {0}", inventory);
                MoveNext();
                return;
            }

            //_output.WriteLine("Найдена карточка");
            int mfn = _dal.UpdateRecord(inventory, CurrentCard.Id);
            _output.WriteLine("Карточка обновлена, MFN={0}", mfn);
            MoveNext();
        }

        private void MoveNext()
        {
            _numberBox.Clear();
            int index = _listBox.SelectedIndex;
            if (index < _listBox.Items.Count - 1)
            {
                index++;
                _listBox.SelectedIndex = index;
            }
            else
            {
                MessageBox.Show
                    (
                        this,
                        "Ящик закончился!",
                        "Ура!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
            }
        }

        private void _numberBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CheckInventory(_numberBox.Text);
                e.Handled = true;
            }
        }
    }
}
