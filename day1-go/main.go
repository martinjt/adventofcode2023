package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
	"unicode"
)

var numberWords = map[int]string{
	1: "one", 2: "two", 3: "three", 4: "four",
	5: "five", 6: "six", 7: "seven", 8: "eight", 9: "nine",
}

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

	firstOutput, err := getOutput(lines, false, false)
	if err != nil {
		fmt.Println(err)
		return
	}
	secondOutput, err := getOutput(lines, true, true)
	if err != nil {
		fmt.Println(err)
		return
	}
	fmt.Println("First output:", firstOutput)
	fmt.Println("Second output:", secondOutput)
}

func getOutput(lines []string, allowNumberString bool, withOutput bool) (int, error) {
	output := 0
	for lineNumber, line := range lines {
		parsedNumber, err := parseLineForNumbers(line, allowNumberString)
		if err != nil {
			return -1, fmt.Errorf("error parsing line %d: %s", lineNumber, err)
		}
		output += parsedNumber
	}
	return output, nil
}

func parseLineForNumbers(line string, supportNumbersAsWords bool) (int, error) {
	first := -1
	last := -1

	for characterIndex, character := range line {
		if unicode.IsDigit(character) {
			digit := int(character)
			last = digit
			if first == -1 {
				first = digit
			}
		}

		if !supportNumbersAsWords {
			continue
		}

		restOfLine := line[characterIndex:]
		for number, word := range numberWords {
			if strings.HasPrefix(restOfLine, word) {
				last = number
				if first == -1 {
					first = number
				}
			}
		}
	}
	if last == -1 {
		return first, nil
	}

	stringAnswer := fmt.Sprintf("%c%c", first, last)
	value, err := strconv.Atoi(stringAnswer)
	if err != nil {
		return -1, fmt.Errorf("error parsing %s: %s", stringAnswer, err)
	}
	return value, nil
}
