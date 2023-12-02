package main

import (
	"testing"

	"gotest.tools/assert"
)

func TestParsing(t *testing.T) {
	var input = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"

	game, _ := generateGameFromGameLine(input)

	assert.Equal(t, game.GameNumber, 1)
	assert.Equal(t, game.MaxAvailableCubes["blue"], 6)
	assert.Equal(t, game.MaxAvailableCubes["red"], 4)
	assert.Equal(t, game.MaxAvailableCubes["green"], 2)
	assert.Equal(t, game.isAcceptable(map[string]int{"red": 1, "green": 2}), true)
	assert.Equal(t, game.isAcceptable(map[string]int{"red": 1, "green": 3}), false)
}
