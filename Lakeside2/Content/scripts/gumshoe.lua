function gumshoe_faceplayer()
	l:getEntity("gumshoe"):faceEntity(player)
end

function gumshoe_follow()
	l:followPlayer(l:getEntity("gumshoe"))
end

if Flags.getIntFlag("feed_gumshoe") <= 0 then
	local target = player:getFacingTile()
	local gumshoe = l:getEntity("gumshoe")
	l:makeChain(
		l:SNPCDialog(gumshoe, "Bark! Bark!"), l:SMove(gumshoe, target), l:SFunction(gumshoe_faceplayer),
		l:SNPCDialog(gumshoe, "Bark! Bark!"), l:SDialog("Gumshoe looks hungry. I think I left his food in the living room..."),
		l:SFunction(gumshoe_follow)
	)
	Flags.setIntFlag("feed_gumshoe", 1)
end