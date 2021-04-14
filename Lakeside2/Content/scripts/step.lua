function step_howdy()
	api:makeChain(api:SDialog("Well, howdy to you too partner!"))
end

function step_no()
	api:makeChain(api:SDialog("Well, screw you too I guess.\nHave a bad day."))
end

local target = player:getFacingTile()
api:makeChain(api:SDialog("Hey! You there!"), api:SMove(api:getEntity("knight"), target), api:SBranch("You can't just stroll past without sayin' howdy.", "Howdy.", "Sure I can?", step_howdy, step_no))

