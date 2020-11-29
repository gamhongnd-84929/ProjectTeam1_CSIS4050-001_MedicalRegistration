﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MedicalCentreCodeFirstFromDB;

using ProjectTeam01MedicalCentreManagement;

namespace MedicalCentreMainMenuFormApp
{
    public partial class MedicalCentreAllRecordsForm : Form
    {
        public MedicalCentreAllRecordsForm()
        {

            InitializeComponent();
            Text = "Medical Centre: All Records";
            Load += (s, e) => MedicalCentreAllRecordsForm_Load();
            // create child forms
            MedicalCentreAddPatient addPatient = new MedicalCentreAddPatient();

            MedicalCentreAddPractitioner addPractitioner = new MedicalCentreAddPractitioner();

            // add events to buttons
            buttonAddPatient.Click += (s, e) => AddNewUserForm<Customer>(dataGridViewPatients, addPatient);
            buttonAddPractitioner.Click += (s, e) => AddNewUserForm<Practitioner>(dataGridViewPractitioners, addPractitioner);
            buttonPatientOptions.Click += (s, e) => AddingPatientOptionsForm();
            buttonPractitionerOptions.Click += (s, e) => AddingPractitionerOptionsForm();
        }

        private void AddingPractitionerOptionsForm()
        {
            if (dataGridViewPractitioners.SelectedRows.Count != 1)
            {
                MessageBox.Show("Please Select a Practitioner to View their Options");
            }
            else
            {
                int practitionerIdToView = Convert.ToInt32(dataGridViewPractitioners.SelectedRows[0].Cells[0].Value);
                MedicalCentrePractitionerOptionsMainForm practitionerOptionsMainForm = new MedicalCentrePractitionerOptionsMainForm(practitionerIdToView);
                practitionerOptionsMainForm.ShowDialog();

                // reload the datagridview
                ReloadPractitionersRecordsView(dataGridViewPractitioners);
                dataGridViewPractitioners.Refresh();

                // hide the child form
                practitionerOptionsMainForm.Hide();
            }
        }

        private void AddingPatientOptionsForm()
        {
            if (dataGridViewPatients.SelectedRows.Count != 1)
            {
                MessageBox.Show("Please Select a Patient to View their Options");
            }
            else
            {
                int patientIdToView = Convert.ToInt32(dataGridViewPatients.SelectedRows[0].Cells[0].Value);
                MedicalCentrePatientOptionsMainForm patientOptionsMainForm = new MedicalCentrePatientOptionsMainForm(patientIdToView);
                patientOptionsMainForm.ShowDialog();

                // reload the datagridview
                ReloadPatientsRecordsView(dataGridViewPatients);
                dataGridViewPatients.Refresh();


                // hide the child form
                patientOptionsMainForm.Hide();
            }
        }

        private void AddNewUserForm<T>(DataGridView dataGridView, Form form) where T : class
        {
            // if okay was clicked on the child
            var result = form.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Cancel)
            {
                // reload the datagridview
                if (typeof(T) == typeof(Customer))
                {
                    ReloadPatientsRecordsView(dataGridView);
                }
                if (typeof(T) == typeof(Practitioner))
                {
                    ReloadPractitionersRecordsView(dataGridView);
                }
                dataGridView.Refresh();

            }

            // hide the child form
            form.Hide();
        }

        private void MedicalCentreAllRecordsForm_Load()
        {


            InitializePatientsRecordsView(dataGridViewPatients);
            //InitializeDataGridView<Customer>(dataGridViewPatients, "Bookings", "Payments", "User");
            InitializePractitionersRecordsView(dataGridViewPractitioners);

        }

        /// <summary>
        /// Set up the practitionersRecordsView columns and populate data into the view
        /// </summary>
        /// <param name="datagridview"></param>
        private void InitializePractitionersRecordsView(DataGridView datagridview)
        {

            // set number of columns
            datagridview.ColumnCount = 7;
            // set the column header names
            datagridview.Columns[0].Name = "Practitioner ID";
            datagridview.Columns[1].Name = "Type";
            datagridview.Columns[2].Name = "First Name";
            datagridview.Columns[3].Name = "Last Name";
            datagridview.Columns[4].Name = "City";
            datagridview.Columns[5].Name = "Province";
            datagridview.Columns[6].Name = "Email";
            // using unit-of-work context
            using (MedicalCentreManagementEntities context = new MedicalCentreManagementEntities())
            {
                // loop through all practitioners
                foreach (Practitioner practitioner in context.Practitioners)
                {
                    // get the needed information
                    string[] rowAdd = { practitioner.PractitionerID.ToString(), practitioner.Practitioner_Types.Title, practitioner.User.FirstName, practitioner.User.LastName, practitioner.User.City, practitioner.User.Province, practitioner.User.Email };
                    // add to display
                    datagridview.Rows.Add(rowAdd);
                }

            }
            // set all properties
            datagridview.AllowUserToAddRows = false;
            datagridview.AllowUserToDeleteRows = false;
            datagridview.ReadOnly = true;
            datagridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ReloadPractitionersRecordsView(DataGridView datagridview)
        {
            datagridview.Rows.Clear();
            using (MedicalCentreManagementEntities context = new MedicalCentreManagementEntities())
            {
                // loop through all practitioners
                foreach (Practitioner practitioner in context.Practitioners)
                {
                    // get the needed information
                    string[] rowAdd = { practitioner.PractitionerID.ToString(), practitioner.Practitioner_Types.Title, practitioner.User.FirstName, practitioner.User.LastName, practitioner.User.City, practitioner.User.Province, practitioner.User.Email };
                    // add to display
                    datagridview.Rows.Add(rowAdd);
                }

            }
        }

        private static void InitializePatientsRecordsView(DataGridView datagridview)
        {

            // set number of columns
            datagridview.ColumnCount = 8;
            // Set the column header names.
            datagridview.Columns[0].Name = "Customer ID";
            datagridview.Columns[1].Name = "First Name";
            datagridview.Columns[2].Name = "Last Name";
            datagridview.Columns[3].Name = "Address";
            datagridview.Columns[4].Name = "City";
            datagridview.Columns[5].Name = "Province";
            datagridview.Columns[6].Name = "Email";
            datagridview.Columns[7].Name = "Phone Number";
            // using unit-of-work context
            using (MedicalCentreManagementEntities context = new MedicalCentreManagementEntities())
            {
                // loop through all customers
                foreach (Customer customer in context.Customers)
                {
                    if (customer.CustomerID == 6) continue;
                    // get the needed information
                    string[] rowAdd = { customer.CustomerID.ToString(), customer.User.FirstName, customer.User.LastName, customer.User.Address, customer.User.City, customer.User.Province, customer.User.Email, customer.User.PhoneNumber };
                    // add to display
                    datagridview.Rows.Add(rowAdd);
                }

            }
            // set all properties
            datagridview.AllowUserToAddRows = false;
            datagridview.AllowUserToDeleteRows = false;
            datagridview.ReadOnly = true;
            datagridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void ReloadPatientsRecordsView(DataGridView datagridview)
        {
            datagridview.Rows.Clear();
            using (MedicalCentreManagementEntities context = new MedicalCentreManagementEntities())
            {
                // loop through all customers
                foreach (Customer customer in context.Customers)
                {
                    if (customer.CustomerID == 6) continue;
                    // get the needed information
                    string[] rowAdd = { customer.CustomerID.ToString(), customer.User.FirstName, customer.User.LastName, customer.User.Address, customer.User.City, customer.User.Province, customer.User.Email, customer.User.PhoneNumber };
                    // add to display
                    datagridview.Rows.Add(rowAdd);
                }

            }
        }
    }


}
