local Building = CS.TileCtrl.EType.Building
local Empty = CS.TileCtrl.EType.Land
local last = {i = 0, j = 0, sum = 0, op = 0, score = 0}
function OnTurn(self)
    local de = CS.UnityEngine.Debug
    local best = {i = 0, j = 0, sum = 0, op = 0, score = 0}
    local sum = 0;
    for i = 0, 10 do
        for j = 0, 10 do
            if (self:CanMove(i,j))
            then
                if(self:map(i,j).Type == Empty and self:CanBuild(i,j)) then
                    local score = self:EmptyAround(i,j) * 40
                    if(score >= best.sum and not (self.MyScore == last.score and last.i == i && last.j == j)) then
                        best.op = 1
                        best.i = i
                        best.j = j
                        best.sum = score
                    end
                elseif(self:map(i,j).Type == Building and self:map(i,j).Player~=self.Player and self:CanBuild(i,j)) then
                    local score = (7 - self:EmptyAround(i,j)) * 20
                    if(score >= best.sum and not (self.MyScore == last.score and last.i == i && last.j == j)) then
                        best.op = 1
                        best.i = i
                        best.j = j
                        best.sum = score
                    end
                elseif(self:map(i,j).Type == Building and self:map(i,j).Player==self.Player and self:CanBuild(i,j)) then
                    local score = self:EmptyAround(i,j) * 8
                    if(score >= best.sum and not (self.MyScore == last.score and last.i == i && last.j == j)) then
                        best.op = 1
                        best.i = i
                        best.j = j
                        best.sum = score
                    end
                end
            end
        end 
    end
    best.score = self.MyScore
    de.Log("---")
    de.Log(self.Player)
    de.Log(best.i)
    de.Log(best.j)
    self:Action(best.i,best.j,best.op)
    last = best
end
