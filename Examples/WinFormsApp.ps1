# Пример подключения к серверу ИРБИС64 и поиска записей
# из приложения с графическим пользовательским интерфейсом
# с использованием WinForms и ManagedIrbis

# Загружаем WinForms
Add-Type -AssemblyName System.Windows.Forms

# Загружаем ManagedIrbis
$asmPath = [System.IO.Path]::Combine($PSScriptRoot, "ManagedIrbis.dll")
Add-Type -Path $asmPath

# ==============================================================

# Создаём форму
$form = New-Object System.Windows.Forms.Form
$form.Text = "Поиск читателей"

$panel = New-Object System.Windows.Forms.TableLayoutPanel
$form.Controls.Add($panel)
$panel.ColumnCount = 2
$panel.RowCount = 3
$panel.Dock = [System.Windows.Forms.DockStyle]::Fill

$rowStyle = New-Object System.Windows.Forms.RowStyle
$panel.RowStyles.Add($rowStyle) | Out-Null
$rowStyle = New-Object System.Windows.Forms.RowStyle
$panel.RowStyles.Add($rowStyle) | Out-Null
$rowStyle = New-Object System.Windows.Forms.RowStyle
$rowStyle.SizeType = [System.Windows.Forms.SizeType]::Percent
$rowStyle.Height = 100
$panel.RowStyles.Add($rowStyle) | Out-Null


$columnStyle = New-Object System.Windows.Forms.ColumnStyle
$panel.ColumnStyles.Add($columnStyle) | Out-Null
$columnStyle = New-Object System.Windows.Forms.ColumnStyle
$columnStyle.SizeType = [System.Windows.Forms.SizeType]::Percent
$columnStyle.Width = 100
$panel.ColumnStyles.Add($columnStyle) | Out-Null

$label = New-Object System.Windows.Forms.Label
$label.Text = "№ билета"
$label.AutoSize = $True
$label.Dock = [System.Windows.Forms.DockStyle]::Bottom
$panel.Controls.Add($label,0,0)

$box = New-Object System.Windows.Forms.TextBox
$box.Dock = [System.Windows.Forms.DockStyle]::Top
$panel.Controls.Add($box, 1, 0) | Out-Null

$button = New-Object System.Windows.Forms.Button
$button.Dock = [System.Windows.Forms.DockStyle]::Top
$button.Text = "Найти читателя"
$panel.Controls.Add($button, 1, 1) | Out-Null
$form.AcceptButton = $button

$result = New-Object System.Windows.Forms.TextBox
$result.Multiline = $True
$result.ReadOnly = $True
$result.Dock = [System.Windows.Forms.DockStyle]::Fill
$panel.Controls.Add($result, 0, 2)
$panel.SetColumnSpan($result, 2)

# ==============================================================

# Обработчик нажатия на кнопку "Найти читателя"
$button.add_Click({
    $result.Clear()
    $ticket = $box.Text.Trim()
    if ([System.String]::IsNullOrEmpty($ticket))
    {
        $result.Text = "Не указан номер читательского билета"

        return
    }

    $client = New-Object ManagedIrbis.IrbisConnection
    $client.Host = "127.0.0.1"
    $client.Port = 6666
    $client.Username = "1"
    $client.Password = "1"
    $client.Database = "RDR"
    $client.Connect()
    try
    {
        $found = $client.Search('"RI=' + $ticket + '"')
        if ($found.Length -eq 0)
        {
            $result.Text = "Читатель с таким билетом не найден"

            return
        }

        $mfn = $found[0]
        $record = [ManagedIrbis.IrbisConnectionUtility]::ReadRecord($client, $mfn)
        if ([System.Object]::ReferenceEquals($record, $null))
        {
            $result.Text = "Не удалось загрузить запись"

            return
        }

        $reader = [ManagedIrbis.Readers.ReaderInfo]::Parse($record)
        $result.Text = $reader.ToString()
    }
    finally
    {
        $client.Dispose()
    }
})

# ==============================================================

# Запускаем приложение
[System.Windows.Forms.Application]::Run($form)