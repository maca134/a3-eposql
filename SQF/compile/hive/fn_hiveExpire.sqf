private ['_table', '_instanceId', '_expire', '_packet'];

_table = [_this, 0, '', ['']] call BIS_fnc_param;
_instanceId = [_this, 1, '', ['']] call BIS_fnc_param;
_expire = [_this, 2, 0, [0]] call BIS_fnc_param;

if (_table == '' || _instanceId == '' || _expire == 0) exitWith {};

_packet = ('EXPIRE' + EPOSQL_SEP + _table + EPOSQL_SEP + _id + EPOSQL_SEP + _expire + EPOSQL_SEP);

EPOSQL_DLL callExtension _packet;