package main

import (
	"testing"
)

func TestParsing(t *testing.T) {
	var inputMap = map[string]int{
		"ckmb52fldxkseven3fkjgcbzmnr7":            57,
		"gckhqpb6twoqnjxqplthree2fourkspnsnzxlz1": 61,
		"2onetwocrgbqm7":                          27,
		"frkh2nineqmqxrvdsevenfive":               25,
	}
	for input, expected := range inputMap {
		actual, err := parseLineForNumbers(input, true)
		if err != nil {
			t.Errorf("Error parsing %s: %s", input, err)
		}
		if actual != expected {
			t.Errorf("Expected %d, got %d", expected, actual)
		}
	}
}
