package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"
)

type Game struct {
	GameNumber        int
	MaxAvailableCubes map[string]int
}

var gameLineRegex = regexp.MustCompile(`^Game (?P<gameNumber>\d+): (?P<turns>.*)$`)
var colourInTurnRegex = regexp.MustCompile(`^\s*(?P<number>\d+) (?P<colour>\w+)$`)

func main() {
	if len(os.Args) < 2 {
		fmt.Println("Please provide an input file")
		return
	}
	inputFile := os.Args[1]
	debug := len(os.Args) > 2 && os.Args[2] == "debug"

	bytes, err := os.ReadFile(inputFile)
	if err != nil {
		fmt.Println(err)
		return
	}

	fileAsString := string(bytes)
	lines := strings.Split(fileAsString, "\n")

	games := []Game{}
	for _, line := range lines {
		game, err := generateGameFromGameLine(line)
		if err != nil {
			fmt.Println(err)
			return
		}
		games = append(games, game)
	}

	var runningSumOfAcceptableGames int
	var runningSumOfPowerOfMinCubes int
	for _, game := range games {
		if debug {
			game.printGameDebug()
		}

		if game.isAcceptable(map[string]int{"red": 12, "green": 13, "blue": 14}) {
			runningSumOfAcceptableGames += game.GameNumber
		}

		runningSumOfPowerOfMinCubes += game.MaxAvailableCubes["red"] * game.MaxAvailableCubes["green"] * game.MaxAvailableCubes["blue"]
	}

	fmt.Printf("Sum of Acceptable games %v\n", runningSumOfAcceptableGames)
	fmt.Printf("Sum of Power of Min Cubes %v\n", runningSumOfPowerOfMinCubes)
}

func generateGameFromGameLine(gameString string) (Game, error) {
	parsedGameLine := gameLineRegex.FindStringSubmatch(gameString)

	gameNumber, err := strconv.Atoi(parsedGameLine[1])
	if err != nil {
		return Game{}, fmt.Errorf("error parsing game number: %s", err)
	}
	game := Game{
		GameNumber:        gameNumber,
		MaxAvailableCubes: make(map[string]int),
	}

	turnsString := parsedGameLine[2]
	turns := strings.Split(turnsString, ";")

	for _, turn := range turns {
		coloursInTurn := strings.Split(turn, ",")
		for _, colourResult := range coloursInTurn {
			parsedColourResult := colourInTurnRegex.FindStringSubmatch(colourResult)

			colourName := parsedColourResult[2]
			cubeForColourInTurn, err := strconv.Atoi(parsedColourResult[1])
			if err != nil {
				return Game{}, fmt.Errorf("error parsing cube for colour in turn: %s", err)
			}
			currentMaxCubesForColour := game.MaxAvailableCubes[colourName]

			if currentMaxCubesForColour == 0 || currentMaxCubesForColour < cubeForColourInTurn {
				game.MaxAvailableCubes[colourName] = cubeForColourInTurn
			}
		}
	}

	return game, nil
}

func (game *Game) isAcceptable(availableColours map[string]int) bool {
	for colour, available := range availableColours {
		if game.MaxAvailableCubes[colour] > available {
			return false
		}
	}
	return true
}

func (game *Game) printGameDebug() {
	fmt.Printf("Game number: %v Colour Requirements %v\n", game.GameNumber, game.MaxAvailableCubes)
}
