function step_howdy()
	l:makeChain(l:SDialog("Well, howdy to you too partner!"))
end

function step_no()
	l:makeChain(l:SDialog("Well, screw you too I guess.\nHave a bad day."))
end

function step_faceplayer()
	l:getEntity("knight"):faceEntity(player)
end

local target = player:getFacingTile()
l:makeChain(
	l:SDialog("Hey! You there!"), l:SMove(l:getEntity("knight"), target), l:SFunction(step_faceplayer), 
	l:SBranch("You can't just stroll past without sayin' howdy.", "Howdy.", "Sure I can?", step_howdy, step_no)
)