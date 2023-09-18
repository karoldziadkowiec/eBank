using System;
using System.Collections.Generic;
using System.Linq;
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
        public float checkingAccount { get; set; }
        public float savingsAccount { get; set; }
        public int activity { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string idCardNumber { get; set; }
        public string placeOfBirth { get; set; }
        public string residentialAddress { get; set; }
        public string correspondenceAddress { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string passwordReminder { get; set; }
        public float withdrawalLimit { get; set; }
        public float transactionLimit { get; set; }
        public string creationDate { get; set; }
        public string cardNumber { get; set; }
        public int cardActivity { get; set; }
        public string cardColor { get; set; }
        public string cardStartDate { get; set; }
        public string cardEndDate { get; set; }

        public Client(int ID, string ACCOUNTTYPE, string PESELNUMBER, string NAME, string SURNAME,
              string LOGIN, string PASSWORD, float CHECKINGACCOUNT, float SAVINGSACCOUNT,
              int ACTIVITY, string GENDER, string BIRTHDAY, string IDCARDNUMBER,
              string PLACEOFBIRTH, string RESIDENTIALADDRESS, string CORRESPONDENCEADDRESS,
              string EMAIL, string PHONENUMBER, string PASSWORDREMINDER,
              float WITHDRAWALLIMIT, float TRANSACTIONLIMIT, string CREATIONDATE,
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
            correspondenceAddress = CORRESPONDENCEADDRESS;
            email = EMAIL;
            phoneNumber = PHONENUMBER;
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
