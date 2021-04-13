local target = player:getFacingTile()
api:makeChain(api:SDialog("Hey! You there!"), api:SMove(api:getEntity("greg"), target), api:SDialog("You can't just stroll past without sayin' howdy."))

