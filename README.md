# eBank(in progress)

Technologies used in the project: C#, .NET Framework (WPF), XAML, Microsoft SQL Server.

To-do list:
- registration: choice between admin and customer, customer data -> contact details (address, correspondence address, telephone number, e-mail) -> login details, sys. where you forget your password, acceptance of the regulations
- login: choice between admin and client, login, password,
- Home: date top right, two tabs, + and - next to it. under the history of the last 5 transfers, next to this month's expenses
- History: displaying a table with transfers, printing transaction history in PDF, filtering if necessary
- Transfers: deposit, withdrawal, regular transfer, own transfer, BLIK, phone top-up, currency transfer, tax transfer
- Services: order a card, expense analysis, transfer request, phone top-up, game top-up and gift cards, transport tickets, motorway tickets, parking tickets
- Settings: blocking, limits,
- Profile: profile details, my finances, edit profile, delete profile
- Log out: question box


Bazy danych:
- clients: id, pesel, name, surname, login, password, sex, birthday, idCardNumber, placeOfBirth, residentialAddress, correspondenceAddress, telNumber, email, creationDate
- accountData: id, clientID, checkingAccount, savingsAccount
- eBankData: id, name, value
- settings: id, accountType, clientID, activity, card, question, withdrawalLimit, transactionLimit
- cards: id, clientID, number, activity, color, startDate, endDate
- transactions: id, senderID, recipientID, value, type, title, description, date
- transactionType: id, name(Deposit, Withdrawal, Regular transfer, Own transfer, BLIK, Phone top-up, Currency transfer, Tax transfer)
- phoneTariffs: id, name
- currencies: id, name, percent
- taxes: id, name
- games: id, name, value
- giftCards: id, name, value
- transportTickets: id, name, type, time, value
- highwayTickets: id, name, type, value
- parkingTickets: id, name, time, value
