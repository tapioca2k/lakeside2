-- decide which map to load
if TimeOfDay.getMillis() < (1000 * 60 * 60 * 8) then
	overworld:loadMap("Content/map/mapnight.json")
else
	overworld:loadMap("Content/map/map.json")
end

-- add extra locations
if Flags.getBooleanFlag("said_howdy") then
	overworld:addLocation("town.json", LuaAPI.makeVector2(400, 100))
end