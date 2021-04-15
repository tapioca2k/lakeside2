function step_howdy()
	api:makeChain(api:SDialog("Well, howdy to you too partner!"))
end

function step_no()
	api:makeChain(api:SDialog("Well, screw you too I guess.\nHave a bad day."))
end

function step_faceplayer()
	api:getEntity("knight"):faceEntity(player)
end

local target = player:getFacingTile()
api:makeChain(
	api:SDialog("Hey! You there!"), api:SMove(api:getEntity("knight"), target), api:SFunction(step_faceplayer), 
	api:SBranch("You can't just stroll past without sayin' howdy.", "Howdy.", "Sure I can?", step_howdy, step_no)
)
