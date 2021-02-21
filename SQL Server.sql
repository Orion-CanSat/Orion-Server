SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+02:00";

DROP TABLE IF EXISTS `Altitude`;
CREATE TABLE IF NOT EXISTS `Altitude` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `AltitudeM` double NOT NULL,
  `AltitudeF` double NOT NULL,
  `Notice` text CHARACTER SET utf8 COLLATE utf8_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `Humidity`;
CREATE TABLE IF NOT EXISTS `Humidity` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `RelativeHumidity` double NOT NULL,
  `Notice` text COLLATE utf8_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `Pressure`;
CREATE TABLE IF NOT EXISTS `Pressure` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `PressurePa` double NOT NULL,
  `PressurekPa` double NOT NULL,
  `Notice` text COLLATE utf8_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `Temperature`;
CREATE TABLE IF NOT EXISTS `Temperature` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `TemperatureC` double NOT NULL,
  `TemperatureF` double NOT NULL,
  `Notice` text COLLATE utf8_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

COMMIT;