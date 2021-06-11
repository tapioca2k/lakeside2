function facePlayer()
	l:getEntity("amogusred"):faceEntity(player)
end

local target = player:getFacingTile()
l:makeChain(
	l:SMove(l:getEntity("amogusred"), target),
	l:SFunction(facePlayer),
	l:SDialog("moko seems kinda sus")
)
Flags.setBooleanFlag("following", true)
l:followPlayer(l:getEntity("amogusred"))