-- decide which map to load
if TimeOfDay.getHours() < 6 or TimeOfDay.getHours() > 20 then
	overworld:loadMap("Content/map/mapnight.json")
else
	overworld:loadMap("Content/map/map.json")
end

-- add extra locations
if Flags.getBooleanFlag("said_howdy") then
	overworld:addLocation("town.json", LuaAPI.makeVector2(400, 100))
end