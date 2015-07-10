private ['_table', '_instanceId', '_packet'];

_table = [_this, 0, '', ['']] call BIS_fnc_param;
_instanceId = [_this, 1, '', ['']] call BIS_fnc_param;

if (_table == '' || _instanceId == '') exitWith {};

_packet = ('DEL' + EPOSQL_SEP + _table + EPOSQL_SEP + _instanceId + EPOSQL_SEP);

EPOSQL_DLL callExtension _packet;