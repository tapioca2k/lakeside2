-- decide which map to load

-- Respect time of day system
--[[
if Flags.getBooleanFlag("started_game") then
	if TimeOfDay.getHours() < 6 or TimeOfDay.getHours() > 20 then
		overworld:loadMap("Content/map/mapnight.json")
	else
		overworld:loadMap("Content/map/map.json")
	end
else
	overworld:loadMap("Content/map/map.json")
	Flags.setBooleanFlag("started_game", true)
end
]]

-- Just load the main map
overworld:loadMap("Content/map/map.json")
Flags.setBooleanFlag("started_game", true)


-- add extra locations
--[[
if Flags.getBooleanFlag("said_howdy") then
	overworld:addLocation("town.json", LuaAPI.makeVector2(400, 100))
end
]]
