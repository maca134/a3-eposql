private ['_log'];
if (!EPOSQL_LOG) exitWith {};
_log = _this;

if (typeName _log != 'STRING') exitWith {};

diag_log format['EPOSQL: %1', _log];