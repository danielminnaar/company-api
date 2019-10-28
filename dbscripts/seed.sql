\connect companydb
CREATE TABLE companies
(
 id serial PRIMARY KEY,
 name VARCHAR (150) NOT NULL,
 exchange VARCHAR (150) NOT NULL,
 ticker VARCHAR (10) NOT NULL,
 isin VARCHAR (12) NOT NULL,
 website VARCHAR (255) NOT NULL
);
ALTER TABLE companies OWNER TO companyuser;
Insert into companies(name,exchange, ticker, isin, website) values(‘Apple Inc.’,'NASDAQ', 'AAPL', 'US0378331005', 'http://www.apple.com');
Insert into companies(name,exchange, ticker, isin, website) values(‘British Airways Plc’,'Pink Sheets', 'BAIRY', 'US1104193065', '');
Insert into companies(name,exchange, ticker, isin, website) values(‘Heineken NV’,'Euronext Amsterdam', 'HEIA', 'NL0000009165', '');
