# Sorting Algorithm Visualiser 📊

A C# application that visually demonstrates how different sorting algorithms work by animating the process of sorting a collection of values.

The project was created to improve my understanding of sorting algorithms while exploring how algorithms can be represented visually through a graphical application.

## 🎯 Project Goals

The main goals of this project are to:

* Implement common sorting algorithms from scratch.
* Visualise how sorting algorithms manipulate data.
* Compare different sorting approaches.
* Practise algorithmic thinking and problem solving.
* Gain experience developing an interactive graphical application in C#.

## 🛠️ Technologies

* **C#**
* **.NET**
* **MonoGame**
* **Visual Studio**
* **Git / GitHub**

## 📊 Sorting Algorithms

The project currently includes visualisations for sorting algorithms such as:

* Bubble Sort
* Insertion Sort

Each algorithm operates on a collection of values represented visually as blocks.

The blocks change position during the sorting process, allowing the user to see how the algorithm works rather than simply receiving the final sorted result.

## 👀 Visualisation

The application represents values as blocks with different heights.

For example:

```text
        █                 █
        █       █         █   █
        █       █         █   █
    █   █       █         █   █   █
    █   █   █   █  ────>  █   █   █   █
    █   █   █   █         █   █   █   █
    █   █   █   █         █   █   █   █
```

As the algorithm runs, the blocks are compared and rearranged until the collection is sorted.

This provides a visual representation of operations that would otherwise only be visible through code.

## 🧠 What I Learned

This project has helped me develop my understanding of:

* Sorting algorithms
* Algorithm complexity
* Arrays and collections
* Loops and iteration
* Swapping values
* Algorithm visualisation
* Game/application loops
* Event handling
* C# programming
* Debugging interactive applications

It has also helped me understand that an algorithm's behaviour can be represented in different ways and that visualisation can make complex processes easier to understand.

## 🚧 Current Limitations

The project is currently a work in progress.

Known limitations include:

* ❌ No main menu
* ⚠️ Controls/buttons are not currently clearly indicated
* ⚠️ Block textures still need to be added
* ⚠️ User interface requires further improvement
* ⚠️ More sorting algorithms could be added
* ⚠️ The application needs clearer instructions for new users

These are planned improvements rather than completed features.

## 🔮 Future Development

### User Interface

* [ ] Add a main menu
* [ ] Add clear instructions for controls
* [ ] Add buttons for selecting sorting algorithms
* [ ] Add a start/pause
* [ ] Add a visualisation speed control
* [ ] Display the currently selected algorithm
* [ ] Display algorithm statistics

### Visual Improvements

* [ ] Add proper textures for the sorting blocks
* [ ] Improve block animations
* [ ] Improve overall UI design
* [ ] Add visual indicators when blocks are being compared
* [ ] Add visual indicators when blocks are swapped

### Algorithms

* [ ] Add more sorting algorithms
* [ ] Add merge sort
* [ ] Add quick sort

### Information & Statistics

* [ ] Display number of comparisons
* [ ] Display number of swaps
* [ ] Display elapsed time
* [ ] Display Big-O complexity
* [ ] Allow algorithms to be compared using the same dataset

## 📈 Algorithm Complexity

The project can eventually be used to demonstrate the difference in performance between sorting algorithms.

| Algorithm      |  Best Case | Average Case | Worst Case |     Space |
| -------------- | ---------: | -----------: | ---------: | --------: |
| Bubble Sort    |       O(n) |        O(n²) |      O(n²) |      O(1) |
| Insertion Sort |       O(n) |        O(n²) |      O(n²) |      O(1) |
| Selection Sort |      O(n²) |        O(n²) |      O(n²) |      O(1) |
| Merge Sort     | O(n log n) |   O(n log n) | O(n log n) |      O(n) |
| Quick Sort     | O(n log n) |   O(n log n) |      O(n²) | O(log n)* |

* Average auxiliary space for a typical recursive implementation.

## 🖥️ Planned User Experience

The intended user flow is:

```text
Main Menu
    │
    ├── Select Algorithm
    │
    ├── Configure Dataset
    │
    └── Start Visualisation
              │
              ▼
       Sorting Visualiser
              │
       ┌──────┴──────┐
       │             │
    Compare──Draw───Swap
       │             │
       └──────┬──────┘
              │
              ▼
         Sorted Data
```

## 📚 Why I Built This

Sorting algorithms are commonly taught using code and complexity calculations, but it can be difficult to understand exactly what is happening during execution.

This project explores how sorting can be visualised so that comparisons and swaps can be observed directly.

It also provides an opportunity to experiment with different algorithms and see how their behaviour changes as the size and order of the input data changes.

## 📌 Project Status

**In Development 🚧**

The core sorting visualisation is being developed, with further work planned on the user interface, visual assets, controls and additional algorithms.

## 👨‍💻 Author

**Dylan Wright**

Computer Science student interested in software development, C#, algorithms, data structures and game development.

GitHub: [Dylan-Thomas-Wright](https://github.com/Dylan-Thomas-Wright)
