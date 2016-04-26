Add-Type -Assembly 'System.Windows.Forms' -ErrorAction Stop;
Add-Type -Assembly 'System.Drawing' -ErrorAction Stop;
Import-Module 'Erwine.Leonard.T.IOUtility' -ErrorAction Stop;
Import-Module 'Erwine.Leonard.T.XmlUtility' -ErrorAction Stop;

$Script:CredentialsPath = 'C:\Users\leonarde\Documents\AppData\Credentials.xml';
$Script:CredentialsXmlDocument = Read-XmlDocument -InputUri $Script:CredentialsPath;

Function Show-EditForm {
    [CmdletBinding(DefaultParameterSetName = 'Edit')]
    Param(
        [Parameter(ParameterSetName = 'Edit')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Delete')]
        [System.Xml.XmlElement]$XmlElement,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Delete')]
        [switch]$Delete
    )
    
    $Edit = $PSBoundParameters.ContainsKey('XmlElement');
    $Form = New-Object -TypeName 'System.Windows.Forms.Form' -Property @{
        Name = 'PasswordStorageEdit{0}' -f [System.Guid]::NewGuid().ToString('N');
        TopLevel = $true;
        Size = New-Object -TypeName 'System.Drawing.Size' -ArgumentList 800, 600;
    };
    if ($Edit) {
        $SelectedId = [System.Xml.XmlConvert]::ToInt32((Get-AttributeText -XmlElement $XmlElement -Name 'ID'));
    } else {
        $XmlElement = $Script:CredentialsXmlDocument.DocumentElement.AppendChild($Script:CredentialsXmlDocument.CreateElement('Credential'));
        $SelectedId = 0;
        while ($Script:CredentialsXmlDocument.SelectSingleNode(('/Credentials/Credential[@ID="{0}"]' -f $SelectedId)) -ne $null) { $SelectedId++ }
        Set-AttributeText -XmlElement $XmlElement -Name 'ID' -Value ([System.Xml.XmlConvert]::ToString($SelectedId));
    }
    try {
        $InteractionProperties = @{
            SelectedId = $SelectedId;
            SelectedName = Get-ElementText -XmlElement $XmlElement -Name 'Name';
            XmlElement = $XmlElement;
            mainForm = $Form;
            outerTableLayoutPanel = New-Object -TypeName 'System.Windows.Forms.TableLayoutPanel' -Property @{
                Name = 'outerTableLayoutPanel';
                Dock = [System.Windows.Forms.DockStyle]::Fill;
                AutoSize = $true;
            };
            nameHeadingLabel = New-Object -TypeName 'System.Windows.Forms.Label' -Property @{
                Name = 'nameHeadingLabel';
                Anchor = [System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Right;
                Text = 'Name:'
                AutoSize = $true;
            };
            nameTextBox = New-Object -TypeName 'System.Windows.Forms.TextBox' -Property @{
                Name = 'nameTextBox';
                Dock = [System.Windows.Forms.DockStyle]::Top;
            };
            nameErrorLabel = New-Object -TypeName 'System.Windows.Forms.Label' -Property @{
                Name = 'nameErrorLabel';
                Anchor = [System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Left;
                ForeColor = [System.Drawing.Color]::Red;
                Text = 'Name cannot be empty.';
                AutoSize = $true;
            };
            loginHeadingLabel = New-Object -TypeName 'System.Windows.Forms.Label' -Property @{
                Name = 'loginHeadingLabel';
                Anchor = [System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Right;
                Text = 'Login:'
                AutoSize = $true;
            };
            loginTextBox = New-Object -TypeName 'System.Windows.Forms.TextBox' -Property @{
                Name = 'loginTextBox';
                Dock = [System.Windows.Forms.DockStyle]::Top;
            };
            passwordHeadingLabel = New-Object -TypeName 'System.Windows.Forms.Label' -Property @{
                Name = 'passwordHeadingLabel';
                Anchor = [System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Right;
                Text = 'Password:'
                AutoSize = $true;
            };
            passwordTextBox = New-Object -TypeName 'System.Windows.Forms.TextBox' -Property @{
                Name = 'passwordTextBox';
                Dock = [System.Windows.Forms.DockStyle]::Top;
                UseSystemPasswordChar = $true;
            };
            confirmHeadingLabel = New-Object -TypeName 'System.Windows.Forms.Label' -Property @{
                Name = 'confirmHeadingLabel';
                Anchor = [System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Right;
                Text = 'Confirm:'
                AutoSize = $true;
            };
            confirmTextBox = New-Object -TypeName 'System.Windows.Forms.TextBox' -Property @{
                Name = 'confirmTextBox';
                Dock = [System.Windows.Forms.DockStyle]::Top;
                UseSystemPasswordChar = $true;
            };
            passwordErrorLabel = New-Object -TypeName 'System.Windows.Forms.Label' -Property @{
                Name = 'passwordErrorLabel';
                Anchor = [System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Left;
                ForeColor = [System.Drawing.Color]::Red;
                Text = 'Password and confirmation do not match.';
                Visible = $false;
                AutoSize = $true;
            };
            urlHeadingLabel = New-Object -TypeName 'System.Windows.Forms.Label' -Property @{
                Name = 'urlHeadingLabel';
                Anchor = [System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Left;
                Text = 'Url:'
                AutoSize = $true;
            };
            urlTextBox = New-Object -TypeName 'System.Windows.Forms.TextBox' -Property @{
                Name = 'urlTextBox';
                Dock = [System.Windows.Forms.DockStyle]::Top;
            };
            notesHeadingLabel = New-Object -TypeName 'System.Windows.Forms.Label' -Property @{
                Name = 'notesHeadingLabel';
                Anchor = [System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Left;
                Text = 'Notes:'
                AutoSize = $true;
            };
            notesTextBox = New-Object -TypeName 'System.Windows.Forms.TextBox' -Property @{
                Name = 'notesTextBox';
                Dock = [System.Windows.Forms.DockStyle]::Fill;
                WordWrap = $true;
                AutoSize = $true;
                AcceptsTab = $true;
                AcceptsReturn = $true;
                Multiline = $true;
                ScrollBars = [System.Windows.Forms.ScrollBars]::Vertical;
            };
            buttonTableLayoutPanel = New-Object -TypeName 'System.Windows.Forms.TableLayoutPanel' -Property @{
                Name = 'buttonTableLayoutPanel';
                Dock = [System.Windows.Forms.DockStyle]::Fill;
                AutoSize = $true;
            };
            okButton = New-Object -TypeName 'System.Windows.Forms.Button' -Property @{
                Name = 'okButton';
                DialogResult = [System.Windows.Forms.DialogResult]::OK;
                Anchor = [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right;
            };
            cancelButton = New-Object -TypeName 'System.Windows.Forms.Button' -Property @{
                Name = 'cancelButton';
                DialogResult = [System.Windows.Forms.DialogResult]::Cancel;
                Anchor = [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right;
            };
        };

        if ($Delete) {
            $InteractionProperties.okButton.Text = 'Yes';
            $InteractionProperties.cancelButton.Text = 'No';
            $InteractionProperties.mainForm.Text = 'Delete "{0}"' -f $InteractionProperties.SelectedName;
        } else {
            $InteractionProperties.okButton.Text = 'OK';
            $InteractionProperties.cancelButton.Text = 'Cancel';
            if ($Edit) {
                $InteractionProperties.mainForm.Text = 'Edit "{0}"' -f $InteractionProperties.SelectedName;
            } else {
                $InteractionProperties.mainForm.Text = 'New Credential';
            }
        }
        
        $InteractionProperties.mainForm.Tag = $InteractionProperties;
        $InteractionProperties.nameTextBox.Tag = $InteractionProperties;
        $InteractionProperties.urlTextBox.Tag = $InteractionProperties;
        $InteractionProperties.loginTextBox.Tag = $InteractionProperties;
        $InteractionProperties.passwordTextBox.Tag = $InteractionProperties;
        $InteractionProperties.confirmTextBox.Tag = $InteractionProperties;
        $InteractionProperties.notesTextBox.Tag = $InteractionProperties;
        $InteractionProperties.okButton.Tag = $InteractionProperties;
        $InteractionProperties.cancelButton.Tag = $InteractionProperties;

        $InteractionProperties.mainForm.Controls.Add($InteractionProperties.outerTableLayoutPanel);
        $InteractionProperties.mainForm.AcceptButton = $InteractionProperties.okButton;
        $InteractionProperties.mainForm.CancelButton = $InteractionProperties.cancelButton;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::Percent, 50))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::Percent, 50))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::Percent, 100))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.nameHeadingLabel, 0, 0);
        $InteractionProperties.outerTableLayoutPanel.SetRowSpan($InteractionProperties.nameHeadingLabel, 2);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.nameTextBox, 1, 0);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.nameErrorLabel, 1, 1);
        
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.nameHeadingLabel, 0, 0);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.nameTextBox, 1, 0);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.loginHeadingLabel, 2, 0);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.loginTextBox, 3, 0);
        
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.nameErrorLabel, 1, 1);
        $InteractionProperties.outerTableLayoutPanel.SetColumnSpan($InteractionProperties.nameErrorLabel, 3);
        
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.passwordHeadingLabel, 0, 2);
        $InteractionProperties.outerTableLayoutPanel.SetRowSpan($InteractionProperties.passwordHeadingLabel, 2);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.passwordTextBox, 1, 2);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.confirmHeadingLabel, 2, 2);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.confirmTextBox, 3, 2);
        
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.passwordErrorLabel, 1, 3);
        $InteractionProperties.outerTableLayoutPanel.SetColumnSpan($InteractionProperties.passwordErrorLabel, 3);
        
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.urlHeadingLabel, 0, 4);
        $InteractionProperties.outerTableLayoutPanel.SetColumnSpan($InteractionProperties.urlHeadingLabel, 4);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.urlTextBox, 0, 5);
        $InteractionProperties.outerTableLayoutPanel.SetColumnSpan($InteractionProperties.urlTextBox, 4);
        
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.notesHeadingLabel, 0, 6);
        $InteractionProperties.outerTableLayoutPanel.SetColumnSpan($InteractionProperties.notesHeadingLabel, 6);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.notesTextBox, 0, 7);
        $InteractionProperties.outerTableLayoutPanel.SetColumnSpan($InteractionProperties.notesTextBox, 4);
        
        $InteractionProperties.buttonTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::Percent, 100))) | Out-Null;
        $InteractionProperties.buttonTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.buttonTableLayoutPanel, 1, 8);
        $InteractionProperties.outerTableLayoutPanel.SetColumnSpan($InteractionProperties.buttonTableLayoutPanel, 4);
        
        $InteractionProperties.buttonTableLayoutPanel.Controls.Add($InteractionProperties.okButton, 0, 0);
        $InteractionProperties.buttonTableLayoutPanel.Controls.Add($InteractionProperties.cancelButton, 1, 0);
        
        $InteractionProperties.mainForm.add_Shown({
            Param(
                [Parameter(Mandatory = $true, Position = 0)]
                [object]$Sender,
                
                [Parameter(Mandatory = $true, Position = 0)]
                [System.EventArgs]$E
            )
            
            $Sender.Focus();
        });

        if (-not $Delete) {
            $InteractionProperties.nameTextBox.add_TextChanged({
                Param(
                    [Parameter(Mandatory = $true, Position = 0)]
                    [object]$Sender,
                    
                    [Parameter(Mandatory = $true, Position = 0)]
                    [System.EventArgs]$E
                )
                
                $InteractionProperties = $Sender.Tag;
                $ErrorMessage = $null;
                if ($InteractionProperties.nameTextBox.Text.Trim() -eq '') {
                    $ErrorMessage = 'Name cannot be empty.';
                } else {
                    for ($s = $XmlElement.PreviousSibling; $s -ne $null; $s = $s.PreviousSibling) {
                        if ($s -is [System.Xml.XmlElement] -and (Get-ElementText -XmlElement $s -Name 'Name').Trim() -ieq $InteractionProperties.nameTextBox.Text.Trim()) {
                            $ErrorMessage = 'Another item has the same name.';
                            break;
                        }
                    }
                    if ($ErrorMessage -eq $null) {
                        for ($s = $XmlElement.NextSibling; $s -ne $null; $s = $s.NextSibling) {
                            if ($s -is [System.Xml.XmlElement] -and (Get-ElementText -XmlElement $s -Name 'Name').Trim() -ieq $InteractionProperties.nameTextBox.Text.Trim()) {
                                $ErrorMessage = 'Another item has the same name.';
                                break;
                            }
                        }
                    }
                }
                
                if ($ErrorMessage -ne $null) {
                    $InteractionProperties.nameErrorLabel.Text = 'Name cannot be empty.';
                    $InteractionProperties.nameErrorLabel.Visible = $true;
                    $InteractionProperties.okButton.Enabled = $false;
                } else {
                    $InteractionProperties.nameErrorLabel.Visible = $false;
                    $InteractionProperties.okButton.Enabled = -not $InteractionProperties.passwordErrorLabel.Visible;
                }
            });

            $InteractionProperties.passwordTextBox.add_TextChanged({
                Param(
                    [Parameter(Mandatory = $true, Position = 0)]
                    [object]$Sender,
                    
                    [Parameter(Mandatory = $true, Position = 0)]
                    [System.EventArgs]$E
                )
                
                $InteractionProperties = $Sender.Tag;
                if ($InteractionProperties.passwordTextBox.Text -ceq $InteractionProperties.confirmTextBox.Text) {
                    $InteractionProperties.passwordErrorLabel.Visible = $false;
                    $InteractionProperties.okButton.Enabled = -not $InteractionProperties.nameErrorLabel.Visible;
                } else {
                    $InteractionProperties.passwordErrorLabel.Visible = $true;
                    $InteractionProperties.okButton.Enabled = $false;
                }
            });

            $InteractionProperties.confirmTextBox.add_TextChanged({
                Param(
                    [Parameter(Mandatory = $true, Position = 0)]
                    [object]$Sender,
                    
                    [Parameter(Mandatory = $true, Position = 0)]
                    [System.EventArgs]$E
                )
                
                $InteractionProperties = $Sender.Tag;
                if ($InteractionProperties.passwordTextBox.Text -ceq $InteractionProperties.confirmTextBox.Text) {
                    $InteractionProperties.passwordErrorLabel.Visible = $false;
                    $InteractionProperties.okButton.Enabled = -not $InteractionProperties.nameErrorLabel.Visible;
                } else {
                    $InteractionProperties.passwordErrorLabel.Visible = $true;
                    $InteractionProperties.okButton.Enabled = $false;
                }
            });
        }
        
        if ($PSBoundParameters.ContainsKey('XmlElement')) {
            $InteractionProperties.nameTextBox.Text = $InteractionProperties.SelectedName;
            $InteractionProperties.urlTextBox.Text = Get-ElementText -XmlElement $XmlElement -Name 'Url';
            $InteractionProperties.loginTextBox.Text = Get-ElementText -XmlElement $XmlElement -Name 'Login';
            $Password = Get-ElementText -XmlElement $XmlElement -Name 'Password';
            if ($Password -ne '') {
                $PSCredential = New-Object -TypeName 'System.Management.Automation.PSCredential' -ArgumentList (Get-ElementText -XmlElement $XmlElement -Name 'Login'), ($Password | ConvertTo-SecureString);
                $InteractionProperties.passwordTextBox.Text = $PSCredential.GetNetworkCredential().Password;
                $InteractionProperties.confirmTextBox.Text = $InteractionProperties.passwordTextBox.Text;
            }
            $InteractionProperties.notesTextBox.Text = Get-ElementText -XmlElement $XmlElement -Name 'Notes';
            if ($Delete) {
                $InteractionProperties.nameTextBox.ReadOnly = $true;
                $InteractionProperties.nameErrorLabel.Text = 'Are you sure you want to delete this item?';
                $InteractionProperties.loginTextBox.ReadOnly = $true;
                $InteractionProperties.passwordTextBox.Visible = $false;
                $InteractionProperties.urlTextBox.ReadOnly = $true;
                $InteractionProperties.notesTextBox.ReadOnly = $true;
            }
        }
        
        if ($InteractionProperties.mainForm.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) {
            if ($Delete) {
                $XmlElement.ParentNode.RemoveChild($XmlElement) | Out-Null;
            } else {
                Set-ElementText -XmlElement $XmlElement -Name 'Name' -Value $InteractionProperties.nameTextBox.Text;
                Set-ElementText -XmlElement $XmlElement -Name 'Login' -Value $InteractionProperties.loginTextBox.Text;
                if ($InteractionProperties.passwordTextBox.Text.Trim().Length -eq 0) {
                    Set-ElementText -XmlElement $XmlElement -Name 'Password' -Value '';
                } else {
                    $SecureString = $InteractionProperties.passwordTextBox.Text | ConvertTo-SecureString -AsPlainText -Force;
                    Set-ElementText -XmlElement $XmlElement -Name 'Password' -Value ($SecureString | ConvertFrom-SecureString);
                }
                Set-ElementText -XmlElement $XmlElement -Name 'Url' -Value $InteractionProperties.urlTextBox.Text;
                Set-ElementText -XmlElement $XmlElement -Name 'Notes' -Value $InteractionProperties.notesTextBox.Text;
            }
            Save-CredentialsDocument;
        } else {
            if (-not ($Edit -or $Delete)) { $XmlElement.ParentNode.RemoveChild($XmlElement) | Out-Null }
        }
    } catch {
        if (-not ($Edit -or $Delete)) { $XmlElement.ParentNode.RemoveChild($XmlElement) | Out-Null }
        throw;
    } finally {
        $Form.Dispose();
    }
}

Function Show-BrowseForm {
    Param()
    
    if ([System.Windows.Forms.Clipboard]::ContainsText()) {
        $TextDataFormat = @(@(
            [System.Windows.Forms.TextDataFormat]::Rtf,
            [System.Windows.Forms.TextDataFormat]::Html,
            [System.Windows.Forms.TextDataFormat]::CommaSeparatedValue,
            [System.Windows.Forms.TextDataFormat]::UnicodeText,
            [System.Windows.Forms.TextDataFormat]::Text
        ) | Where-Object { [System.Windows.Forms.Clipboard]::ContainsText($_) });
        if ($TextDataFormat.Count -gt 0) {
            $ClipboardText = [System.Windows.Forms.Clipboard]::GetText($TextDataFormat[0]);
        } else {
            $ClipboardText = [System.Windows.Forms.Clipboard]::GetText();
        }
    } else {
        $ClipboardText = $null;
    }
    $Form = New-Object -TypeName 'System.Windows.Forms.Form' -Property @{
        Name = 'PasswordStorageBrowseForm{0}' -f [System.Guid]::NewGuid().ToString('N');
        Text = "Credential Listing";
        TopLevel = $true;
        Size = New-Object -TypeName 'System.Drawing.Size' -ArgumentList 800, 600;
    };
    try {
        $InteractionProperties = @{
            SelectedId = $null;
            DataSource = New-Object -TypeName 'System.Data.DataTable';
            IdDataColumn = New-Object -TypeName 'System.Data.DataColumn' -Property @{
                AllowDBNull = $false;
                Caption = 'ID';
                ColumnName = 'ID';
                DataType = [int];
                Unique = $true;
            }
            NameDataColumn = New-Object -TypeName 'System.Data.DataColumn' -Property @{
                AllowDBNull = $false;
                Caption = 'Name';
                ColumnName = 'Name';
                DataType = [string];
            }
            LoginDataColumn = New-Object -TypeName 'System.Data.DataColumn' -Property @{
                AllowDBNull = $false;
                Caption = 'Login';
                ColumnName = 'Login';
                DataType = [string];
            }
            UrlDataColumn = New-Object -TypeName 'System.Data.DataColumn' -Property @{
                AllowDBNull = $false;
                Caption = 'Url';
                ColumnName = 'Url';
                DataType = [string];
            }
            PasswordDataColumn = New-Object -TypeName 'System.Data.DataColumn' -Property @{
                AllowDBNull = $false;
                Caption = 'Password';
                ColumnName = 'Password';
                DataType = [string];
            }
            mainForm = $Form;
            outerTableLayoutPanel = New-Object -TypeName 'System.Windows.Forms.TableLayoutPanel' -Property @{
                Name = 'outerTableLayoutPanel';
                Dock = [System.Windows.Forms.DockStyle]::Fill;
                AutoSize = $true;
            };
            mainDataGridView = New-Object -TypeName 'System.Windows.Forms.DataGridView' -Property @{
                Name = 'mainDataGridView';
                Dock = [System.Windows.Forms.DockStyle]::Fill;
                ReadOnly = $true;
                AllowUserToAddRows = $false;
                AllowUserToDeleteRows = $false;
                AutoGenerateColumns = $false;
                AutoSize = $true;
                MultiSelect = $true;
                SelectionMode = [System.Windows.Forms.DataGridViewSelectionMode]::CellSelect
                #SelectionMode = [System.Windows.Forms.DataGridViewSelectionMode]::FullRowSelect
            };
            copyPasswordButton = New-Object -TypeName 'System.Windows.Forms.Button' -Property @{
                Name = 'copyPasswordButton';
                Text = 'Copy PW';
                Anchor = [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right;
            };
            editButton = New-Object -TypeName 'System.Windows.Forms.Button' -Property @{
                Name = 'editButton';
                Text = 'Edit';
                DialogResult = [System.Windows.Forms.DialogResult]::Yes;
                Anchor = [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right;
            };
            duplicateButton = New-Object -TypeName 'System.Windows.Forms.Button' -Property @{
                Name = 'duplicateButton';
                Text = 'Duplicate';
                Anchor = [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right;
            };
            newButton = New-Object -TypeName 'System.Windows.Forms.Button' -Property @{
                Name = 'newButton';
                Text = 'New';
                DialogResult = [System.Windows.Forms.DialogResult]::No;
                Anchor = [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right;
            };
            deleteButton = New-Object -TypeName 'System.Windows.Forms.Button' -Property @{
                Name = 'deleteButton';
                Text = 'Delete';
                DialogResult = [System.Windows.Forms.DialogResult]::Abort;
                Anchor = [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right;
            };
            exitButton = New-Object -TypeName 'System.Windows.Forms.Button' -Property @{
                Name = 'exitButton';
                Text = 'Exit';
                DialogResult = [System.Windows.Forms.DialogResult]::Cancel;
                Anchor = [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right;
            };
            idDataGridColumn = New-Object -TypeName 'System.Windows.Forms.DataGridViewTextBoxColumn' -Property @{
                DataPropertyName = 'ID';
                HeaderText = 'ID';
                ReadOnly = $true;
                AutoSizeMode = [System.Windows.Forms.DataGridViewAutoSizeColumnMode]::AllCells;
                SortMode = [System.Windows.Forms.DataGridViewColumnSortMode]::Automatic;
            };
            nameDataGridColumn = New-Object -TypeName 'System.Windows.Forms.DataGridViewTextBoxColumn' -Property @{
                DataPropertyName = 'Name';
                HeaderText = 'Name';
                ReadOnly = $true;
                AutoSizeMode = [System.Windows.Forms.DataGridViewAutoSizeColumnMode]::AllCells;
                SortMode = [System.Windows.Forms.DataGridViewColumnSortMode]::Automatic;
            };
            loginDataGridColumn = New-Object -TypeName 'System.Windows.Forms.DataGridViewTextBoxColumn' -Property @{
                DataPropertyName = 'Login';
                HeaderText = 'Login';
                ReadOnly = $true;
                AutoSizeMode = [System.Windows.Forms.DataGridViewAutoSizeColumnMode]::AllCells;
                SortMode = [System.Windows.Forms.DataGridViewColumnSortMode]::Automatic;
            };
            urlDataGridColumn = New-Object -TypeName 'System.Windows.Forms.DataGridViewTextBoxColumn' -Property @{
                DataPropertyName = 'Url';
                HeaderText = 'Url';
                ReadOnly = $true;
                AutoSizeMode = [System.Windows.Forms.DataGridViewAutoSizeColumnMode]::Fill;
                SortMode = [System.Windows.Forms.DataGridViewColumnSortMode]::Automatic;
            };
            SelectionChangedScriptBlock = {
                Param(
                    [Parameter(Mandatory = $true, Position = 0)]
                    [object]$Sender,
                    
                    [Parameter(Mandatory = $true, Position = 0)]
                    [System.EventArgs]$E
                )
                
                $InteractionProperties = $Sender.Tag;
                if ($InteractionProperties.CurrentRowCellStyle -eq $null) { return }
                
                $RowIndexes = @($InteractionProperties.mainDataGridView.SelectedCells | ForEach-Object { $_.RowIndex });
                if ($InteractionProperties.mainDataGridView.SelectedCells.Count -eq 1) {
                    foreach ($r in $InteractionProperties.mainDataGridView.Rows) {
                        if ($r.Index -ne $InteractionProperties.mainDataGridView.CurrentCell.RowIndex) {
                            $r.DefaultCellStyle = $InteractionProperties.mainDataGridView.DefaultCellStyle;
                        }
                    }
                    $InteractionProperties.mainDataGridView.CurrentCell.OwningRow.DefaultCellStyle = $InteractionProperties.CurrentRowCellStyle;
                    $InteractionProperties.editButton.Enabled = $true;
                    $InteractionProperties.deleteButton.Enabled = $true;
                    $InteractionProperties.duplicateButton.Enabled = $true;
                    if ($InteractionProperties.mainDataGridView.CurrentCell.OwningRow -ne $null -and $InteractionProperties.mainDataGridView.CurrentCell.OwningRow.DataBoundItem -ne $null) {
                        #$InteractionProperties.mainDataGridView.CurrentCell.OwningRow | Get-Member | Write-Host
                        $Row =  $InteractionProperties.mainDataGridView.CurrentCell.OwningRow.DataBoundItem.Row;
                        $InteractionProperties.copyPasswordButton.Enabled = ($Row[$InteractionProperties.PasswordDataColumn] -ne '');
                    } else {
                        $InteractionProperties.copyPasswordButton.Enabled = $false;
                    }
                } else {
                    $InteractionProperties.copyPasswordButton.Enabled = $false;
                    $InteractionProperties.editButton.Enabled = $false;
                    $InteractionProperties.deleteButton.Enabled = $false;
                    $InteractionProperties.duplicateButton.Enabled = $false;
                }
            };
            TerminalButtonScriptBlock = {
                Param(
                    [Parameter(Mandatory = $true, Position = 0)]
                    [object]$Sender,
                    
                    [Parameter(Mandatory = $true, Position = 0)]
                    [System.EventArgs]$E
                )
                
                $InteractionProperties = $Sender.Tag;
                $InteractionProperties.mainForm.DialogResult = $Sender.DialogResult;
                $InteractionProperties.mainForm.Close();
            };
        };

        $InteractionProperties.mainForm.Tag = $InteractionProperties;
        $InteractionProperties.mainDataGridView.Tag = $InteractionProperties;
        $InteractionProperties.copyPasswordButton.Tag = $InteractionProperties;
        $InteractionProperties.editButton.Tag = $InteractionProperties;
        $InteractionProperties.newButton.Tag = $InteractionProperties;
        $InteractionProperties.deleteButton.Tag = $InteractionProperties;
        $InteractionProperties.exitButton.Tag = $InteractionProperties;
        $InteractionProperties.DataSource.Columns.Add($InteractionProperties.IdDataColumn);
        $InteractionProperties.DataSource.Columns.Add($InteractionProperties.NameDataColumn);
        $InteractionProperties.DataSource.Columns.Add($InteractionProperties.LoginDataColumn);
        $InteractionProperties.DataSource.Columns.Add($InteractionProperties.UrlDataColumn);
        $InteractionProperties.DataSource.Columns.Add($InteractionProperties.PasswordDataColumn);

        $InteractionProperties.mainForm.Controls.Add($InteractionProperties.outerTableLayoutPanel);
        $InteractionProperties.mainForm.AcceptButton = $InteractionProperties.editButton;
        $InteractionProperties.mainForm.CancelButton = $InteractionProperties.exitButton;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::Percent, 100))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.ColumnStyles.Add((New-Object -TypeName 'System.Windows.Forms.ColumnStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::Percent, 100))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.RowStyles.Add((New-Object -TypeName 'System.Windows.Forms.RowStyle' -ArgumentList ([System.Windows.Forms.SizeType]::AutoSize))) | Out-Null;
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.mainDataGridView, 0, 0);
        $InteractionProperties.outerTableLayoutPanel.SetColumnSpan($InteractionProperties.mainDataGridView, 6);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.copyPasswordButton, 0, 1);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.editButton, 1, 1);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.duplicateButton, 2, 1);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.newButton, 3, 1);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.deleteButton, 4, 1);
        $InteractionProperties.outerTableLayoutPanel.Controls.Add($InteractionProperties.exitButton, 5, 1);
        $InteractionProperties.mainDataGridView.Columns.Add($InteractionProperties.idDataGridColumn) | Out-Null;
        $InteractionProperties.mainDataGridView.Columns.Add($InteractionProperties.nameDataGridColumn) | Out-Null;
        $InteractionProperties.mainDataGridView.Columns.Add($InteractionProperties.loginDataGridColumn) | Out-Null;
        $InteractionProperties.mainDataGridView.Columns.Add($InteractionProperties.urlDataGridColumn) | Out-Null;
        
        $XmlNodeList = $Script:CredentialsXmlDocument.SelectNodes('/Credentials/Credential');
        $InteractionProperties.DataSource.Clear();
        for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
            $XmlElement = $XmlNodeList.Item($i);
            $DataRow = $InteractionProperties.DataSource.NewRow();
            $DataRow.BeginEdit();
            $DataRow[$InteractionProperties.IdDataColumn] = [System.Xml.XmlConvert]::ToInt32((Get-AttributeText -XmlElement $XmlElement -Name 'ID'));
            $DataRow[$InteractionProperties.NameDataColumn] = Get-ElementText -XmlElement $XmlElement -Name 'Name';
            $DataRow[$InteractionProperties.LoginDataColumn] = Get-ElementText -XmlElement $XmlElement -Name 'Login';
            $DataRow[$InteractionProperties.UrlDataColumn] = Get-ElementText -XmlElement $XmlElement -Name 'Url';
            $DataRow[$InteractionProperties.PasswordDataColumn] = Get-ElementText -XmlElement $XmlElement -Name 'Password';
            $DataRow.EndEdit();
            $InteractionProperties.DataSource.Rows.Add($DataRow);
            $DataRow.AcceptChanges();
        }
        $InteractionProperties.DataSource.AcceptChanges();

        $InteractionProperties.mainDataGridView.DataSource = $InteractionProperties.DataSource;
        
        $InteractionProperties.mainForm.add_Shown({
            Param(
                [Parameter(Mandatory = $true, Position = 0)]
                [object]$Sender,
                
                [Parameter(Mandatory = $true, Position = 0)]
                [System.EventArgs]$E
            )
            
            $Sender.Focus();
        });

        $InteractionProperties.mainForm.add_FormClosed({
            Param(
                [Parameter(Mandatory = $true, Position = 0)]
                [object]$Sender,
                
                [Parameter(Mandatory = $true, Position = 0)]
                [System.Windows.Forms.FormClosedEventArgs]$E
            )
            
            $InteractionProperties = $Sender.Tag;
            if ($InteractionProperties.mainDataGridView.CurrentCell -ne $null -and $InteractionProperties.mainDataGridView.CurrentCell.OwningRow -ne $null -and $InteractionProperties.mainDataGridView.CurrentCell.OwningRow.DataBoundItem -ne $null) {
                $InteractionProperties.SelectedId = $InteractionProperties.mainDataGridView.CurrentCell.OwningRow.DataBoundItem.Row[$InteractionProperties.IdDataColumn];
            } else {
                $InteractionProperties.SelectedId = $null;
            }
        });
        $InteractionProperties.mainDataGridView.add_DataBindingComplete({
            Param(
                [Parameter(Mandatory = $true, Position = 0)]
                [object]$Sender,
                
                [Parameter(Mandatory = $true, Position = 0)]
                [System.Windows.Forms.DataGridViewBindingCompleteEventArgs]$E
            )
            
            $InteractionProperties = $Sender.Tag;
            if ($InteractionProperties.CurrentRowCellStyle -eq $null) {
                $InteractionProperties.CurrentRowCellStyle = New-Object -TypeName 'System.Windows.Forms.DataGridViewCellStyle' -ArgumentList $InteractionProperties.mainDataGridView.DefaultCellStyle;
                $InteractionProperties.CurrentRowCellStyle.BackColor = [System.Drawing.Color]::LightCoral;
                $InteractionProperties.SelectionChangedScriptBlock.Invoke($sender, ([System.EventArgs]::Empty)) | Out-Null;
            }
        });
        
        $InteractionProperties.mainDataGridView.add_SelectionChanged($InteractionProperties.SelectionChangedScriptBlock);
        
        $InteractionProperties.newButton.add_Click($InteractionProperties.TerminalButtonScriptBlock);
        $InteractionProperties.deleteButton.add_Click($InteractionProperties.TerminalButtonScriptBlock);

        $InteractionProperties.copyPasswordButton.add_Click({
            Param(
                [Parameter(Mandatory = $true, Position = 0)]
                [object]$Sender,
                
                [Parameter(Mandatory = $true, Position = 0)]
                [System.EventArgs]$E
            )
            
            $InteractionProperties = $Sender.Tag;
            if ($InteractionProperties.mainDataGridView.CurrentCell.RowIndex -ge 0 -and $InteractionProperties.mainDataGridView.CurrentCell.RowIndex -lt $InteractionProperties.mainDataGridView.RowCount) {
                $Row =  $InteractionProperties.mainDataGridView.Rows[$InteractionProperties.mainDataGridView.CurrentCell.RowIndex].DataBoundItem.Row;
                $Password = $Row[$InteractionProperties.PasswordDataColumn];
                if ($Password -ne '') {
                    $PSCredential = New-Object -TypeName 'System.Management.Automation.PSCredential' -ArgumentList $Row[$InteractionProperties.LoginDataColumn], ($Password | ConvertTo-SecureString);
                    [System.Windows.Forms.Clipboard]::SetText($PSCredential.GetNetworkCredential().Password, [System.Windows.Forms.TextDataFormat]::Text);
                    $PSCredential = $null;
                } else {
                    [System.Windows.Forms.Clipboard]::SetText('', [System.Windows.Forms.TextDataFormat]::Text);
                }
            }
        });
        
        New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
            DialogResult = $InteractionProperties.mainForm.ShowDialog();
            SelectedId = $InteractionProperties.SelectedId;
        };
    } catch {
        throw;
    } finally {
        if ($ClipboardText -eq $null) {
            [System.Windows.Forms.Clipboard]::SetText('', [System.Windows.Forms.TextDataFormat]::Text);
        } else {
            if ($TextDataFormat.Count -gt 0) {
                [System.Windows.Forms.Clipboard]::SetText($ClipboardText, $TextDataFormat[0]);
            } else {
                [System.Windows.Forms.Clipboard]::SetText($ClipboardText);
            }
        }
        $Form.Dispose();
    }
}

Function Save-CredentialsDocument {
    Param()
    
    $Settings = New-XmlWriterSettings -Indent $true;
    Write-XmlDocument -Document $Script:CredentialsXmlDocument -OutputFileName $Script:CredentialsPath -Settings $Settings;
}

Function Get-ElementText {
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$XmlElement,
        
        [Parameter(Mandatory = $true)]
        [string]$Name
    )
    
    $e = $XmlElement.SelectSingleNode($Name);
    if ($e -eq $null -or $e.IsEmpty) {
        "";
    } else {
        $e.InnerText;
    }
}

Function Get-AttributeText {
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$XmlElement,
        
        [Parameter(Mandatory = $true)]
        [string]$Name
    )
    
    $a = $XmlElement.SelectSingleNode('@' + $Name);
    if ($a -eq $null) {
        "";
    } else {
        $a.value;
    }
}

Function Set-ElementText {
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$XmlElement,
        
        [Parameter(Mandatory = $true)]
        [string]$Name,
        
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$Value
    )
    
    $e = $XmlElement.SelectSingleNode($Name);
    if ($e -eq $null) { $e = $XmlElement.AppendChild($XmlElement.OwnerDocument.CreateElement($Name)) }
    $e.InnerText = $Value
}

Function Set-AttributeText {
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$XmlElement,
        
        [Parameter(Mandatory = $true)]
        [string]$Name,
        
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]$Value
    )
    
    $a = $XmlElement.SelectSingleNode('@' + $Name);
    if ($a -eq $null) { $a = $XmlElement.Attributes.Append($XmlElement.OwnerDocument.CreateAttribute($Name)) }
    $a.Value = $Value
}

Function Get-YesOrNo {
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Caption,
        
        [Parameter(Mandatory = $true)]
        [string]$Message,
        
        [bool]$DefaultValue = $false
    )
    $Choices = New-Object -TypeName 'System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]';
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Yes'));
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_No'));
    $Choices.Add((New-Object -TypeName 'System.Management.Automation.Host.ChoiceDescription' -ArgumentList '_Cancel'));
    if ($DefaultValue) {
        $Index = $Host.UI.PromptForChoice($Caption, $Message, $Choices, 0);
    } else {
        $Index = $Host.UI.PromptForChoice($Caption, $Message, $Choices, 1);
    }
    if ($Index -ne $null) {
        if ($Index -eq 0) {
            $true;
        } else {
            if ($Index -eq 1) { $false }
        }
    }
}

$result = Show-BrowseForm;
while ($result -ne $null -and ($result.DialogResult -ne [System.Windows.Forms.DialogResult]::Cancel)) {
    switch ($result.DialogResult) {
        { $_ -eq [System.Windows.Forms.DialogResult]::Yes } {
            Show-EditForm -XmlElement $Script:CredentialsXmlDocument.SelectSingleNode(('/Credentials/Credential[@ID="{0}"]' -f $result.SelectedId.ToString()));
            break;
        }
        { $_ -eq [System.Windows.Forms.DialogResult]::No } {
            Show-EditForm;
            break;
        }
        { $_ -eq [System.Windows.Forms.DialogResult]::Abort } {
            Show-EditForm -XmlElement $Script:CredentialsXmlDocument.SelectSingleNode(('/Credentials/Credential[@ID="{0}"]' -f $result.SelectedId.ToString())) -Delete;
            break;
        }
    }
    $result = Show-BrowseForm;
}