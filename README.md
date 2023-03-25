# Etherscan Take Home Test
This is a personal project created for take home test given by Etherscan. The requirement can be obtained from here: https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/Etherscan%20Coding%20Test.pdf

## Features
- Single Page Application that are able to view, edit and delete cryptocurrencies without reloading the whole page.
- Dynamic chart to display cryptocurrencies by respective total supply.
- Dynamic table to display cryptocurrencies by respective name, symbol, contract address, total supply, total holders and total supply %.
- Download crpytocurrencies details in .csv files

## Tech
### Frontend
- [AngularJS](https://angularjs.org/)
- [TypeScript](https://www.typescriptlang.org/)
### Backend
- [.NET Core 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [C# 10](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10)
- [MySQL](https://www.mysql.com/)

## Installation
- Requires [Angular 15](https://angular.io/guide/update-to-version-15#update-angular-to-v15) to run
- Running from Visual Studio should install all npm dependencies for you and start both .NET Core web API and Angular web app.

### MySQL Setup
- Create tokens table and prepopulate the data:
```SQL
CREATE TABLE `tokens` (
`id` INT(11) NOT NULL AUTO_INCREMENT,
`symbol` VARCHAR(5) NOT NULL COLLATE 'utf8_general_ci',
`name` VARCHAR(50) NOT NULL COLLATE 'utf8_general_ci',
`total_supply` BIGINT(20) NOT NULL,
`contract_address` VARCHAR(66) NOT NULL COLLATE 'utf8_general_ci',
`total_holders` INT(11) NOT NULL,
`price` DECIMAL(65,2) NULL DEFAULT '0.00',
PRIMARY KEY (`id`) USING BTREE
)
COLLATE='utf8_general_ci'
ENGINE=InnoDB
ROW_FORMAT=DYNAMIC
AUTO_INCREMENT=8;

INSERT INTO `tokens` (`symbol`, `name`, `total_supply`, `contract_address`,
`total_holders`, `price`) VALUES ('VEN', 'Vechain', 35987133,
'0xd850942ef8811f2a866692a623011bde52a462c1', 65, 0.00);
INSERT INTO `tokens` (`symbol`, `name`, `total_supply`, `contract_address`,
`total_holders`, `price`) VALUES ('ZIR', 'Zilliqa', 53272942,
'0x05f4a42e251f2d52b8ed15e9fedaacfcef1fad27', 54, 0.00);
INSERT INTO `tokens` (`symbol`, `name`, `total_supply`, `contract_address`,
`total_holders`, `price`) VALUES ('MKR', 'Maker', 45987133,
'0x9f8f72aa9304c8b593d555f12ef6589cc3a579a2', 567, 0.00);
INSERT INTO `tokens` (`symbol`, `name`, `total_supply`, `contract_address`,
`total_holders`, `price`) VALUES ('BNB', 'Binance', 16579517,
'0xB8c77482e45F1F44dE1745F52C74426C631bDD52', 4234234, 0.00);
```

## Demo
### Main Page
![Main Page with Form and Chart](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_MainPage_1.png?raw=true)
![Main Page with Table](https://github.com/chinyik/token-statistic-viewer/blob/main//Resources/TokenScanner_MainPage_2.png?raw=true)

### View Cryptocurrency
- Can be accessed via:
    -  table (access from Symbol column of eacy cryptocurrency)
    -  chart (access from chart portion of each cryptocurrency)

![View Cryptocurrency from table](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_ViewToken_1.png?raw=true)
![View Cryptocurrency from chart](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_ViewToken_1.png?raw=true)

- The price of each cryptocurrency will be updated every 5 minutes (this value is configurable from appsettings.json)
```json
  "ServiceDelayInHours": 0,
  "ServiceDelayInMinutes": 5,
  "ServiceDelayInSeconds": 0
```

![View Cryptocurrency with updated price](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_ViewToken_UpdatedPrice.png?raw=true)

### Add Cryptocurrency
- Can be added by filling up each input
- Clicking Save button will add the cryptocurrency details in MySQL database and refresh the chart and table data

![Add cryptocurrency](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_AddToken1.png?raw=true)
![New cryptocurrency in the table](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_AddToken2.png?raw=true)
![New cryptocurrency in the chart](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_AddToken3.png?raw=true)

### Edit Cryptocurrency
- Can be accessed by clicking Edit on each cryptocurrency row in the table
- The details of the cryptocurrency will be populated in the inputs
- Clicking Save button will update the details in MySQL database and refresh the chart and table data

![Edit cryptocurrency](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_EditToken_1.png?raw=true)
![Updated chart and table after edit](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_EditToken_2.png?raw=true)

### Delete Cryptocurrency
- Can be accessed by clicking Delete on each cryptocurrency row in the table
- Confirmation will be asked before deleting. Confirming delete will remove cryptocurrency in MySQL database and refresh the chart and table data

![Delete cryptocurrency with confirmation popup](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_DeleteToken_1.png?raw=true)
![Updated chart and table after delete](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_DeleteToken_2.png?raw=true)

### Export Cryptocurrencies
- Can be accessed by clicking Export button, .csv file containing all cryptocurrencies details will be downloaded.
- Sample file: https://github.com/chinyik/token-statistic-viewer/Resources/tokens.csv

![Exported cryptocurrencies](https://github.com/chinyik/token-statistic-viewer/blob/main/Resources/TokenScanner_ExportToken.png?raw=true)
