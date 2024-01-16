using eBank;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBankTests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void ClientConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            int id = 1;
            string accountType = "karol";
            string peselNumber = "12345678901";
            string name = "Karol";
            string surname = "Dziadkowiec";
            string login = "karoldziadkowiec";
            string password = "password";
            double checkingAccount = 1000.0;
            double savingsAccount = 500.0;
            int activity = 1;
            string gender = "Male";
            string birthday = "1990-01-01";
            string idCardNumber = "ABC123456";
            string placeOfBirth = "Cracow";
            string residentialAddress = "Cracow";
            string correspondencyAddress = "Cracow";
            string phoneNumber = "123456789";
            string email = "karol@gmail.com";
            string passwordReminder = "Reminder";
            double withdrawalLimit = 200.0;
            double transactionLimit = 1000.0;
            string creationDate = "2022-01-01";
            string cardNumber = "1234567890123456";
            int cardActivity = 1;
            string cardColor = "Green";
            string cardStartDate = "2022-01-01";
            string cardEndDate = "2023-01-01";

            // Act
            Client client = new Client(
                id, accountType, peselNumber, name, surname, login, password,
                checkingAccount, savingsAccount, activity, gender, birthday, idCardNumber,
                placeOfBirth, residentialAddress, correspondencyAddress, phoneNumber,
                email, passwordReminder, withdrawalLimit, transactionLimit, creationDate,
                cardNumber, cardActivity, cardColor, cardStartDate, cardEndDate
            );

            // Assert
            Assert.AreEqual(id, client.id);
            Assert.AreEqual(accountType, client.accountType);
            Assert.AreEqual(peselNumber, client.peselNumber);
            Assert.AreEqual(name, client.name);
            Assert.AreEqual(surname, client.surname);
            Assert.AreEqual(login, client.login);
            Assert.AreEqual(password, client.password);
            Assert.AreEqual(checkingAccount, client.checkingAccount);
            Assert.AreEqual(savingsAccount, client.savingsAccount);
            Assert.AreEqual(activity, client.activity);
            Assert.AreEqual(gender, client.gender);
            Assert.AreEqual(birthday, client.birthday);
            Assert.AreEqual(idCardNumber, client.idCardNumber);
            Assert.AreEqual(placeOfBirth, client.placeOfBirth);
            Assert.AreEqual(residentialAddress, client.residentialAddress);
            Assert.AreEqual(correspondencyAddress, client.correspondencyAddress);
            Assert.AreEqual(phoneNumber, client.phoneNumber);
            Assert.AreEqual(email, client.email);
            Assert.AreEqual(passwordReminder, client.passwordReminder);
            Assert.AreEqual(withdrawalLimit, client.withdrawalLimit);
            Assert.AreEqual(transactionLimit, client.transactionLimit);
            Assert.AreEqual(creationDate, client.creationDate);
            Assert.AreEqual(cardNumber, client.cardNumber);
            Assert.AreEqual(cardActivity, client.cardActivity);
            Assert.AreEqual(cardColor, client.cardColor);
            Assert.AreEqual(cardStartDate, client.cardStartDate);
            Assert.AreEqual(cardEndDate, client.cardEndDate);
        }

        [TestMethod]
        public void SetterAndGetter_WorkCorrectly()
        {
            // Arrange
            Client client = new Client();

            // Act
            client.name = "Karol";

            // Assert
            Assert.AreEqual("Karol", client.name);
        }
    }
}
