package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
	"unicode"
)

func main() {
	if len(os.Args) < 2 {
		fmt.Println("Please provide an input file")
		return
	}
	inputFile := os.Args[1]

	bytes, err := os.ReadFile(inputFile)
	if err != nil {
		fmt.Println(err)
		return
	}

	fileAsString := string(bytes)
	lines := strings.Split(fileAsString, "\n")

	firstOutput, err := getOutput(lines, false)
	if err != nil {
		fmt.Println(err)
		return
	}
	fmt.Println("First output:", firstOutput)
}

func getOutput(lines []string, allowNumberString bool) (int, error) {
	output := 0
	for _, line := range lines {
		first := -1
		last := -1

		for _, character := range line {
			if unicode.IsDigit(character) {
				digit := int(character)
				last = digit
				if first == -1 {
					first = digit
				}
			}
		}

		value, err := strconv.Atoi(fmt.Sprintf("%c%c", first, last))
		if err != nil {
			return 0, err
		}
		output += value
	}
	return output, nil
}
