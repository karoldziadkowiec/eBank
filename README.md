# eBank(in progress)

Technologies used in the project: C#, .NET Framework (WPF), XAML, Microsoft SQL Server.

To-do list:
- Home: two tabs, next to + and -. under the history of the last 5 transfers, next to this month's expenses
- History: displaying a table with transfers, printing transaction history in PDF, filtering if necessary
- Transfers: deposit, withdrawal, BLIK, phone top-up, currency transfer, tax transfer
- Services: expense analysis, transfer requests, game top-ups and gift cards, transport tickets, motorway tickets, parking tickets
- ADMIN PANEL

Databases:
- clients: id(int), accountType(varchar), peselNumber(varchar), name(varchar), surname(varchar), login(varchar), password(varchar), checkingAccount(float), savingsAccount(float), activity( int), gender(varchar), birthday(datetime), idCardNumber(varchar), placeOfBirth(varchar), residentialAddress(varchar), correspondenceAddress(varchar), email(varchar), telNumber(varchar), passwordReminder(varchar), withdrawalLimit( float), transactionLimit(float), creationDate(datetime), cardNumber(varchar), cardActivity(int), cardColor(varchar), cardStartDate(datetime), cardEndDate(datetime)
- eBankData: id, name, value
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
