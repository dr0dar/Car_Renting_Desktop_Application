using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeAreCars
{
    /*
     * Main user interface for WeAreCars application.
     * It manages authentication, validation, calculations and submission of data.
     */
    public partial class Dashboard : Form
    {
        private Panel loginPanel;
        private Panel mainPanel;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

        // This are Booking controls
        private TextBox txtFirstName;
        private TextBox txtSurname;
        private TextBox txtAddress;
        private TextBox txtAge;
        private CheckBox chkValidLicense;
        private NumericUpDown numDays;
        private ComboBox cbCarType;
        private ComboBox cbFuelType;
        private CheckBox chkUnlimitedMileage;
        private CheckBox chkBreakdownCover;
        private Button btnBook;
        private Label lblTotalCost;
        
        // This are List controls
        private DataGridView dgvBookings;
        private Button btnShowBookings;
        private Button btnNewBooking;

        private Models.User loggedInUser;
        private int loginAttempts = 0;
        
        public Dashboard()
        {
            InitializeComponent();
            Run();
        }


        // Creates the visual parts of the program using code, such as
        // panels, buttons, input boxes, drop-down menus, and their actions/events.
        private void Run()
        {
            this.Text = "WeAreCars Management System";
            this.Size = new Size(800, 600);

            // For Login Panel
            loginPanel = new Panel { Dock = DockStyle.Fill };

            var lblwelcome = new Label { Text = "Welcome to the WeAreCars Booking System", Location = new Point(300, 150), Width = 250 };

            var lblUsername = new Label { Text = "Username:", Location = new Point(250, 200) };
            txtUsername = new TextBox { Location = new Point(350, 200), Width = 150 };
            
            var lblPassword = new Label { Text = "Password:", Location = new Point(250, 240) };
            txtPassword = new TextBox { Location = new Point(350, 240), Width = 150, PasswordChar = '*' };
            
            btnLogin = new Button { Text = "Login", Location = new Point(350, 280), Width = 100 };
            btnLogin.Click += BtnLogin_Click;

            loginPanel.Controls.Add(lblwelcome);
            loginPanel.Controls.Add(lblUsername);
            loginPanel.Controls.Add(txtUsername);
            loginPanel.Controls.Add(lblPassword);
            loginPanel.Controls.Add(txtPassword);
            loginPanel.Controls.Add(btnLogin);

            // Sets up for the main panel that handles booking functions and displays the list of bookings.
            mainPanel = new Panel { Dock = DockStyle.Fill, Visible = false };

            // Initialize ToolTips
            ToolTip toolTip = new ToolTip();

            int yPos = 20;
            int padding = 30;

            mainPanel.Controls.Add(new Label { Text = "First Name*:", Location = new Point(20, yPos) });
            txtFirstName = new TextBox { Location = new Point(150, yPos), Width = 200 };
            toolTip.SetToolTip(txtFirstName, "Enter customer's first name");
            mainPanel.Controls.Add(txtFirstName);
            yPos += padding;

            mainPanel.Controls.Add(new Label { Text = "Surname*:", Location = new Point(20, yPos) });
            txtSurname = new TextBox { Location = new Point(150, yPos), Width = 200 };
            toolTip.SetToolTip(txtSurname, "Enter customer's surname");
            mainPanel.Controls.Add(txtSurname);
            yPos += padding;

            mainPanel.Controls.Add(new Label { Text = "Address*:", Location = new Point(20, yPos) });
            txtAddress = new TextBox { Location = new Point(150, yPos), Width = 200 };
            toolTip.SetToolTip(txtAddress, "Enter customer's address");
            mainPanel.Controls.Add(txtAddress);
            yPos += padding;

            mainPanel.Controls.Add(new Label { Text = "Age*:", Location = new Point(20, yPos) });
            txtAge = new TextBox { Location = new Point(150, yPos), Width = 50 };
            toolTip.SetToolTip(txtAge, "Enter customer's age. Must be a valid number.");
            mainPanel.Controls.Add(txtAge);
            yPos += padding;

            chkValidLicense = new CheckBox { Text = "Valid Driving License*", Location = new Point(150, yPos), Width = 200 };
            toolTip.SetToolTip(chkValidLicense, "Customer MUST have a valid driving license to proceed.");
            mainPanel.Controls.Add(chkValidLicense);
            yPos += padding;

            mainPanel.Controls.Add(new Label { Text = "Days (1-28)*:", Location = new Point(20, yPos) });
            numDays = new NumericUpDown { Location = new Point(150, yPos), Minimum = 1, Maximum = 28, Value = 1 };
            toolTip.SetToolTip(numDays, "Select rental duration (Between 1 and 28 days)");
            mainPanel.Controls.Add(numDays);
            yPos += padding;

            mainPanel.Controls.Add(new Label { Text = "Car Type*:", Location = new Point(20, yPos) });
            cbCarType = new ComboBox { Location = new Point(150, yPos), DropDownStyle = ComboBoxStyle.DropDownList };
            cbCarType.Items.Add("City Car (No extra charge)");
            cbCarType.Items.Add("Family Car (+£50)");
            cbCarType.Items.Add("Sports Car (+£75)");
            cbCarType.Items.Add("SUV (+£65)");
            cbCarType.SelectedIndex = 0;
            toolTip.SetToolTip(cbCarType, "Select the type of car");
            mainPanel.Controls.Add(cbCarType);
            yPos += padding;

            mainPanel.Controls.Add(new Label { Text = "Fuel Type*:", Location = new Point(20, yPos) });
            cbFuelType = new ComboBox { Location = new Point(150, yPos), DropDownStyle = ComboBoxStyle.DropDownList };
            cbFuelType.Items.Add("Petrol (No extra charge)");
            cbFuelType.Items.Add("Diesel (No extra charge)");
            cbFuelType.Items.Add("Hybrid (+£30)");
            cbFuelType.Items.Add("Full Electric (+£50)");
            cbFuelType.SelectedIndex = 0;
            toolTip.SetToolTip(cbFuelType, "Select preferred fuel type");
            mainPanel.Controls.Add(cbFuelType);
            yPos += padding;

            chkUnlimitedMileage = new CheckBox { Text = "Unlimited Mileage (+£10/day)", Location = new Point(150, yPos), Width = 200 };
            mainPanel.Controls.Add(chkUnlimitedMileage);
            yPos += padding;

            chkBreakdownCover = new CheckBox { Text = "Breakdown Cover (+£2/day)", Location = new Point(150, yPos), Width = 200 };
            mainPanel.Controls.Add(chkBreakdownCover);
            yPos += padding;

            btnBook = new Button { Text = "Review && Process Booking", Location = new Point(150, yPos), Width = 200 };
            btnBook.Click += BtnBook_Click;
            mainPanel.Controls.Add(btnBook);
            
            lblTotalCost = new Label { Text = "Total: £0.00", Location = new Point(370, yPos), Font = new Font(this.Font, FontStyle.Bold), AutoSize = true };
            mainPanel.Controls.Add(lblTotalCost);
            
            yPos += 40;

            btnShowBookings = new Button { Text = "View Bookings", Location = new Point(20, yPos), Width = 100 };
            btnShowBookings.Click += (s, e) => { UpdateBookingsList(); dgvBookings.Visible = true; };
            mainPanel.Controls.Add(btnShowBookings);

            btnNewBooking = new Button { Text = "New Booking", Location = new Point(130, yPos), Width = 100 };
            btnNewBooking.Click += (s, e) => { dgvBookings.Visible = false; ClearBookingForm(); };
            mainPanel.Controls.Add(btnNewBooking);

            yPos += 40;

            dgvBookings = new DataGridView 
            { 
                Location = new Point(20, yPos), 
                Size = new Size(700, 200),
                Visible = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            mainPanel.Controls.Add(dgvBookings);

            this.Controls.Add(loginPanel);
            this.Controls.Add(mainPanel);

            // Adds actions/events so the price estimate changes automatically
            // when the user updates values or selections.
            numDays.ValueChanged += UpdatePriceEstimate;
            cbCarType.SelectedIndexChanged += UpdatePriceEstimate;
            cbFuelType.SelectedIndexChanged += UpdatePriceEstimate;
            chkUnlimitedMileage.CheckedChanged += UpdatePriceEstimate;
            chkBreakdownCover.CheckedChanged += UpdatePriceEstimate;
        }


        /// It runs when the Login button click and limits the user to attempts 3 wrong login.
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (loginAttempts >= 3)
            {
                MessageBox.Show("Account locked due to 3 invalid login attempts. Please contact admin.", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = Data.DBConnection.Connect())
                {
                    conn.Open();
                    string query = "SELECT UserID, Username FROM Users WHERE Username = @user AND Password = @pass";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string username = reader.GetString(1);
                                
                                loggedInUser = new Models.User { Id = userId, Username = username }; 
                                loginPanel.Visible = false;
                                mainPanel.Visible = true;
                                UpdatePriceEstimate(null, null);
                                loginAttempts = 0; /// If login is success it reset to 0
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            /// If the program reaches this point, it means no valid user was found or logged in.
            loginAttempts++;
            int remaining = 3 - loginAttempts;
            if (remaining == 0)
            {
                MessageBox.Show("Maximum login attempts reached. Account is now locked.", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = false; /// Disables the button so it cannot be clicked.
            }
            else
            {
                MessageBox.Show($"Invalid username or password. You have {remaining} attempt(s) remaining.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // Checks all input fields, creates a CarBooking object in memory, and shows the details for user confirmation.
        // After confirmation, it connects to the SQL server and saves the final booking record.
        private void BtnBook_Click(object sender, EventArgs e)
        {
            if (!ValidateBookingDetails()) return;

            var booking = new Models.CarBooking
            {
                CustomerFirstName = txtFirstName.Text,
                CustomerSurname = txtSurname.Text,
                CustomerAddress = txtAddress.Text,
                CustomerAge = int.Parse(txtAge.Text),
                HasValidDrivingLicense = chkValidLicense.Checked,
                NumberOfDays = (int)numDays.Value,
                SelectedCarType = (Models.CarType)cbCarType.SelectedIndex,
                SelectedFuelType = (Models.FuelType)cbFuelType.SelectedIndex,
                UnlimitedMileage = chkUnlimitedMileage.Checked,
                BreakdownCover = chkBreakdownCover.Checked,
                StaffID = loggedInUser.Id,
                BookingDate = DateTime.Now
            };

            booking.TotalCost = booking.CalculateTotalCost();
            
            string reviewMessage = $"Please review the booking details:\n\n" +
                                   $"Customer: {booking.CustomerFirstName} {booking.CustomerSurname}\n" +
                                   $"Address: {booking.CustomerAddress}\n" +
                                   $"Age: {booking.CustomerAge}\n" +
                                   $"Days: {booking.NumberOfDays}\n" +
                                   $"Car Type: {cbCarType.SelectedItem}\n" +
                                   $"Fuel: {cbFuelType.SelectedItem}\n" +
                                   $"Unlimited Mileage: {(booking.UnlimitedMileage ? "Yes" : "No")}\n" +
                                   $"Breakdown Cover: {(booking.BreakdownCover ? "Yes" : "No")}\n\n" +
                                   $"Total Cost: £{booking.TotalCost:0.00}\n\n" +
                                   $"Do you want to confirm and save this booking?";
                                   
            var result = MessageBox.Show(reviewMessage, "Review Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    SaveBookingToDatabase(booking);
                    
                    UpdateBookingsList();
                    MessageBox.Show($"Booking processed and saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearBookingForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving to the database:\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        ///Checks all required input fields to ensure they are valid,
        ///such as not being empty, having correct number formats, and meeting age rules.
        ///Shows error messages if something is wrong
        private bool ValidateBookingDetails()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtSurname.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text))
            {
                MessageBox.Show("Please fill in all mandatory fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 18)
            {
                MessageBox.Show("Please enter a valid age (18 or older).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!chkValidLicense.Checked)
            {
                MessageBox.Show("Customer must have a valid driving license to proceed with the booking.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        //Attached to UI event handlers (like checkbox changes or combo box selection).
        //Updates values instantly in real time and refreshes the on-screen breakdown so
        //the staff can see the calculation results immediately.
        private void UpdatePriceEstimate(object sender, EventArgs e)
        {
            int days = (int)numDays.Value;
            decimal baseRate = 25m;
            decimal totalCost = days * baseRate;
            string calculation = $"Base Rate: {days} days x £25 = £{totalCost:0.00}\n";

            switch ((Models.CarType)cbCarType.SelectedIndex)
            {
                case Models.CarType.FamilyCar:
                    totalCost += 50m;
                    calculation += $"Family Car Surcharge: +£50.00\n";
                    break;
                case Models.CarType.SportsCar:
                    totalCost += 75m;
                    calculation += $"Sports Car Surcharge: +£75.00\n";
                    break;
                case Models.CarType.SUV:
                    totalCost += 65m;
                    calculation += $"SUV Surcharge: +£65.00\n";
                    break;
            }

            switch ((Models.FuelType)cbFuelType.SelectedIndex)
            {
                case Models.FuelType.Hybrid:
                    totalCost += 30m;
                    calculation += $"Hybrid Surcharge: +£30.00\n";
                    break;
                case Models.FuelType.FullElectric:
                    totalCost += 50m;
                    calculation += $"Electric Surcharge: +£50.00\n";
                    break;
            }

            if (chkUnlimitedMileage.Checked)
            {
                decimal cost = 10m * days;
                totalCost += cost;
                calculation += $"Unlimited Mileage: {days} days x £10 = +£{cost:0.00}\n";
            }
            if (chkBreakdownCover.Checked)
            {
                decimal cost = 2m * days;
                totalCost += cost;
                calculation += $"Breakdown Cover: {days} days x £2 = +£{cost:0.00}\n";
            }

            calculation += $"\nTotal: £{totalCost:0.00}";
            lblTotalCost.Text = calculation;
        }


        /// Takes the temporary in-memory list and displays it neatly in the data grid view 
        /// so staff can clearly see the current session data.
        private void UpdateBookingsList()
        {
            dgvBookings.DataSource = null;

            try
            {
                var fetchedBookings = new List<object>();

                using (var conn = Data.DBConnection.Connect())
                {
                    conn.Open();
                    string query = @"
                        SELECT BookingID, CustomerFirstName, CustomerSurname, NumberOfDays, CarType, TotalCost, BookingDate 
                        FROM Car_Booking 
                        ORDER BY BookingDate DESC";

                    using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fetchedBookings.Add(new
                            {
                                ID = reader.GetInt32(0),
                                Customer = $"{reader.GetString(1)} {reader.GetString(2)}",
                                Days = reader.GetInt32(3),
                                Car = (Models.CarType)reader.GetInt32(4),
                                Cost = $"£{reader.GetDecimal(5):0.00}",
                                Date = reader.GetDateTime(6).ToShortDateString()
                            });
                        }
                    }
                }

                dgvBookings.DataSource = fetchedBookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load bookings from database: {ex.Message}", "Error loading view", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Uses ADO.NET SQL connection and commands with SQL parameters to 
        // securely save the CarBooking data into the Car_Booking database table.
        private void SaveBookingToDatabase(Models.CarBooking booking)
        {
            using (var conn = Data.DBConnection.Connect())
            {
                conn.Open();
                string query = @"
                    INSERT INTO Car_Booking 
                    (CustomerFirstName, CustomerSurname, CustomerAddress, CustomerAge, 
                     HasValidDrivingLicense, NumberOfDays, CarType, FuelType, 
                     HasUnlimitedMileage, HasBreakdownCover, TotalCost, StaffID, BookingDate)
                    VALUES 
                    (@FirstName, @Surname, @Address, @Age, @License, @Days, 
                     @CarType, @FuelType, @Mileage, @Breakdown, @TotalCost, @StaffID, @Date)";
                     
                using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", booking.CustomerFirstName);
                    cmd.Parameters.AddWithValue("@Surname", booking.CustomerSurname);
                    cmd.Parameters.AddWithValue("@Address", booking.CustomerAddress);
                    cmd.Parameters.AddWithValue("@Age", booking.CustomerAge);
                    cmd.Parameters.AddWithValue("@License", booking.HasValidDrivingLicense);
                    cmd.Parameters.AddWithValue("@Days", booking.NumberOfDays);
                    cmd.Parameters.AddWithValue("@CarType", (int)booking.SelectedCarType);
                    cmd.Parameters.AddWithValue("@FuelType", (int)booking.SelectedFuelType);
                    cmd.Parameters.AddWithValue("@Mileage", booking.UnlimitedMileage);
                    cmd.Parameters.AddWithValue("@Breakdown", booking.BreakdownCover);
                    cmd.Parameters.AddWithValue("@TotalCost", booking.TotalCost);
                    cmd.Parameters.AddWithValue("@StaffID", booking.StaffID);
                    cmd.Parameters.AddWithValue("@Date", booking.BookingDate);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Clears and restores all controls in the booking form back to their original default state
        // (empty fields, default selections, and reset values).
        private void ClearBookingForm()
        {
            txtFirstName.Clear();
            txtSurname.Clear();
            txtAddress.Clear();
            txtAge.Clear();
            chkValidLicense.Checked = false;
            numDays.Value = 1;
            cbCarType.SelectedIndex = 0;
            cbFuelType.SelectedIndex = 0;
            chkUnlimitedMileage.Checked = false;
            chkBreakdownCover.Checked = false;
            UpdatePriceEstimate(null, null);
        }
    }
}
