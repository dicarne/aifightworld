local Building = CS.TileCtrl.EType.Building
local Empty = CS.TileCtrl.EType.Land
function OnTurn(self)
    local de = CS.UnityEngine.Debug
    local best = {i = 0, j = 0, sum = 0, op = 0}
    local sum = 0;
    for i = 0, 10 do
        for j = 0, 10 do
            if (self:CanMove(i,j))
            then
                if(self:map(i,j).Type == Empty and self:CanBuild(i,j)) then
                    local score = self:EmptyAround(i,j) * 40
                    if(score > best.sum) then
                        best.op = 1
                        best.i = i
                        best.j = j
                        best.sum = score
                    end
                elseif(self:map(i,j).Type == Building and self:map(i,j).Player~=self.Player and self:CanBuild(i,j)) then
                    local score = (7 - self:EmptyAround(i,j)) * 20
                    if(score > best.sum) then
                        best.op = 1
                        best.i = i
                        best.j = j
                        best.sum = score
                    end
                elseif(self:map(i,j).Type == Building and self:map(i,j).Player==self.Player and self:CanBuild(i,j)) then
                    local score = self:EmptyAround(i,j) * 8
                    if(score > best.sum) then
                        best.op = 1
                        best.i = i
                        best.j = j
                        best.sum = score
                    end
                end
            end
        end 
    end

    de.Log("---")
    de.Log(self.Player)
    de.Log(best.i)
    de.Log(best.j)
    self:Action(best.i,best.j,best.op)
end











local function fact(t,l)
    if(t==0) then 
        return l
    end
    for i = 0, 10 do
        for j = 0, 10 do
            if (self:CanMove(i,j))
            then
                if(self:map(i,j).GetType == Empty) then
                   local nl = deepadd(l,t,{i,j,0})
                   nl['sum'] = l['sum'] + 20
                   return fact(t-1,nl)
                elseif(self:map(i,j).GetType == Building and self:map(i,j).Player!=self.Player) then
                    local nl = deepadd(l,t,{i,j,1})
                    nl['sum'] = l['sum'] + 30
                    return fact(t-1,nl)
                elseif(self:map(i,j).GetType == Building and self:map(i,j).Player==self.Player) then
                    local nl = deepadd(l,t,{i,j,1})
                    nl['sum'] = l['sum'] + 10
                    return fact(t-1,nl)
                end
            end
        end 
    end
end

function deepadd(list,t,a)
    local newl
    for i=t-1,10,1 do
        newl[i] = list[i]
    end
    newl[t] = a
    return newl
end