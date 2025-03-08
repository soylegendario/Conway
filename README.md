# Conway's Game of Life

This project implements Conway's Game of Life, a cellular automaton devised by the British mathematician John Horton Conway in 1970.

## Overview

Conway's Game of Life is a zero-player game, meaning that its evolution is determined by its initial state, requiring no further input. One interacts with the Game of Life by creating an initial configuration and observing how it evolves.

## Rules

The universe of the Game of Life is an infinite, two-dimensional orthogonal grid of square cells, each of which is in one of two possible states, alive or dead. Every cell interacts with its eight neighbors, which are the cells that are horizontally, vertically, or diagonally adjacent. At each step in time, the following transitions occur:

1. **Underpopulation**: Any live cell with fewer than two live neighbors dies, as if by underpopulation.
2. **Survival**: Any live cell with two or three live neighbors lives on to the next generation.
3. **Overpopulation**: Any live cell with more than three live neighbors dies, as if by overpopulation.
4. **Reproduction**: Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

The initial pattern constitutes the seed of the system. The first generation is created by applying the above rules simultaneously to every cell in the seed—births and deaths occur simultaneously, and the discrete moment at which this happens is sometimes called a tick. Each generation is a pure function of the preceding one.

## Implementation

This project will include:

- A graphical user interface to create and visualize the initial configuration.
- Algorithms to compute the next generation of cells based on the rules.
- Options to start, pause, and reset the simulation.

Stay tuned for updates and further documentation on how to use and contribute to this project.