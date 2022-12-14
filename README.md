# sqlBackup
SQL yedek alır ve mail gönderir.

# Yedek
Birden fazla database yedek alabilir. Program açık kaldığı sürece günlük olarak yedek alıp 7zip ile sıkıştırdıktan sonra json içerisinde ki emaile dosya olarak gönderir.
Şuan için saat aktif değildir.

{
  "Configuration": {
    "backupFolder": "C:\\backup\\",
    "backupLocation": "",
    "databaseServer": ".",
    "databaseUserID": "sa",
    "databasePassword": "123",
    "databaseList": [
      {
        "email1": "test1@gmail.com",
        "email2": "",
        "email3": "",
        "partMB": 100,
        "saat": "21:00",
        "databaseName": "TESTDB"
      },
      {
        "email1": "test1@gmail.com",
        "email2": "",
        "email3": "",
        "partMB": 100,
        "saat": "21:00",
        "databaseName": "TESTDB2"
      }
    ]
  }
}
