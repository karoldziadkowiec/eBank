using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace eBank
{
    public class Client
    {
        public int id { get; set; }
        public string accountType { get; set; }
        public string peselNumber { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public double checkingAccount { get; set; }
        public double savingsAccount { get; set; }
        public int activity { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string idCardNumber { get; set; }
        public string placeOfBirth { get; set; }
        public string residentialAddress { get; set; }
        public string correspondencyAddress { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string passwordReminder { get; set; }
        public double withdrawalLimit { get; set; }
        public double transactionLimit { get; set; }
        public string creationDate { get; set; }
        public string cardNumber { get; set; }
        public int cardActivity { get; set; }
        public string cardColor { get; set; }
        public string cardStartDate { get; set; }
        public string cardEndDate { get; set; }

        public Client()
        {

        }

        public Client(int ID, string ACCOUNTTYPE, string PESELNUMBER, string NAME, string SURNAME,
              string LOGIN, string PASSWORD, double CHECKINGACCOUNT, double SAVINGSACCOUNT,
              int ACTIVITY, string GENDER, string BIRTHDAY, string IDCARDNUMBER,
              string PLACEOFBIRTH, string RESIDENTIALADDRESS, string CORRESPONDENCYADDRESS,
              string PHONENUMBER, string EMAIL, string PASSWORDREMINDER,
              double WITHDRAWALLIMIT, double TRANSACTIONLIMIT, string CREATIONDATE,
              string CARDNUMBER, int CARDACTIVITY, string CARDCOLOR, string CARDSTARTDATE, string CARDENDDATE)
        {
            id = ID;
            accountType = ACCOUNTTYPE;
            peselNumber = PESELNUMBER;
            name = NAME;
            surname = SURNAME;
            login = LOGIN;
            password = PASSWORD;
            checkingAccount = CHECKINGACCOUNT;
            savingsAccount = SAVINGSACCOUNT;
            activity = ACTIVITY;
            gender = GENDER;
            birthday = BIRTHDAY;
            idCardNumber = IDCARDNUMBER;
            placeOfBirth = PLACEOFBIRTH;
            residentialAddress = RESIDENTIALADDRESS;
            correspondencyAddress = CORRESPONDENCYADDRESS;
            phoneNumber = PHONENUMBER;
            email = EMAIL;
            passwordReminder = PASSWORDREMINDER;
            withdrawalLimit = WITHDRAWALLIMIT;
            transactionLimit = TRANSACTIONLIMIT;
            creationDate = CREATIONDATE;
            cardNumber = CARDNUMBER;
            cardActivity = CARDACTIVITY;
            cardColor = CARDCOLOR;
            cardStartDate = CARDSTARTDATE;
            cardEndDate = CARDENDDATE;
        }
    }
}
