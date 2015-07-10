if (!isNil 'EPOSQL_INIT') exitWith {};
EPOSQL_INIT = true;
diag_log "Starting Epoch SQL";
EPOSQL_SEP = toString [10]; // 10 is a newline
EPOSQL_DLL = 'EpoSql';
EPOSQL_LOG = true;

EPOCH_server_hiveDEL 		= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveDEL.sqf";
EPOCH_server_hiveEXPIRE 	= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveEXPIRE.sqf";
EPOCH_server_hiveGET 		= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveGET.sqf";
EPOCH_server_hiveGETRANGE 	= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveGETRANGE.sqf";
EPOCH_server_hiveGETBIT 	= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveGETBIT.sqf";
EPOCH_server_hiveGETTTL 	= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveGETTTL.sqf";
EPOCH_server_hiveLog 		= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveLog.sqf";
EPOCH_server_hiveSET 		= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveSET.sqf";
EPOCH_server_hiveSETBIT 	= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveSETBIT.sqf";
EPOCH_server_hiveSETEX 		= compileFinal preprocessFileLineNumbers "\x\addons\a3_eposql_server\epoch\EPOCH_server_hiveSETEX.sqf";
