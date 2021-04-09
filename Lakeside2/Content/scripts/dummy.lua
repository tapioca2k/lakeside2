-- test script

disp = UiTextDisplay('Greetings from Lua!')
disp:setBackground(LuaAPI.makeColor(255, 255, 255))
api:pushUiElement(disp, 0, 0)