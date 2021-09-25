const desiredAmount = 3;
const setupTime = 20;
const timePerBoard = 15 + 10;
const pricePerHour = 500 + 350;
const hourCostProduction = (timePerBoard/60) * pricePerHour;
const hourCostSetup = (setupTime/60) * pricePerHour;
const rawBoardPrice = 1200;

const rawBoardsNeeded = Math.ceil(desiredAmount / 3);



const price = (hourCostProduction * desiredAmount + hourCostSetup + (rawBoardPrice * rawBoardsNeeded));

console.log(price);




