function step_howdy()
	l:makeChain(l:SDialog("Well, howdy to you too partner!"))
end

function step_no()
	l:followPlayer(l:getEntity("knight"))
	l:makeChain(l:SDialog("I'm not leaving you alone until you say howdy, pal."))
end

function step_faceplayer()
	l:getEntity("knight"):faceEntity(player)
end

if not Flags.getBooleanFlag("forest_stepped") then
	local target = player:getFacingTile()
	l:makeChain(
		l:SDialog("Hey! You there!"), l:SMove(l:getEntity("knight"), target), l:SFunction(step_faceplayer), 
		l:SBranch("You can't just stroll past without sayin' howdy.", "Howdy.", "Sure I can?", step_howdy, step_no)
	)
	Flags.setBooleanFlag("forest_stepped", true)
end