SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+02:00";

DROP TABLE IF EXISTS `altitude`;
CREATE TABLE IF NOT EXISTS `altitude` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `AltitudeM` double NOT NULL,
  `AltitudeF` double NOT NULL,
  `Notice` text CHARACTER SET utf8 COLLATE utf8_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `humidity`;
CREATE TABLE IF NOT EXISTS `humidity` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `RelativeHumidity` double NOT NULL,
  `Notice` text COLLATE utf8_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `pressure`;
CREATE TABLE IF NOT EXISTS `pressure` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `PressurePa` double NOT NULL,
  `PressurekPa` double NOT NULL,
  `Notice` text COLLATE utf8_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `temperature`;
CREATE TABLE IF NOT EXISTS `temperature` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `TemperatureC` double NOT NULL,
  `TemperatureF` double NOT NULL,
  `Notice` text COLLATE utf8_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

COMMIT;