# eBank(in progress)

Technologies used in the project: C#, .NET Framework (WPF), XAML, Microsoft SQL Server.

To-do list:
- Services: expense analysis
- ADMIN PANEL

Databases:
- clients: id(int), accountType(varchar), peselNumber(varchar), name(varchar), surname(varchar), login(varchar), password(varchar), checkingAccount(float), savingsAccount(float), activity(int), gender(varchar), birthday(datetime), idCardNumber(varchar), placeOfBirth(varchar), residentialAddress(varchar), correspondenceAddress(varchar), email(varchar), telNumber(varchar), passwordReminder(varchar), withdrawalLimit(float), transactionLimit(float), creationDate(datetime), cardNumber(varchar), cardActivity(int), cardColor(varchar), cardStartDate(datetime), cardEndDate(datetime)
- transactions: id(int), senderID(int), recipientID(int), value(float), type(int), title(varchar), description(varchar), date(datetime)
- transactionType: id(int), name(varchar):Deposit, Withdrawal, Regular transfer, Own transfer, BLIK, Phone top-up, Currency transfer, Tax transfer
- transferRequest: id(int), senderID(int), recipientID(int), value(float), title(varchar), date(datetime)
- phoneTariffs: id(int), name(varchar)
- currencies: id(int), name(varchar), divisor(float)
- taxes: id(int), name(varchar)
- games: id, name, value
- giftCards: id, name, value
- transportTickets: id, name, type, time, value
- highwayTickets: id, name, type, value
- parkingTickets: id, name, time, value
