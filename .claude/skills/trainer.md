---
name: trainer
description: Invoke the personal running coach agent to analyze your training CSV data and generate a training plan
---

Use the `trainer` agent to analyze my running data and help me reach my training goals.

Read all CSV files in `./data/` and do the following:

1. **Parse and summarize** all available sessions — total distance, average pace, average HR, cadence, and calories per run.
2. **Identify trends** across sessions: is pace improving? Is HR dropping at the same effort? Is cadence in the optimal range?
3. **Flag any issues** such as HR consistently too high, pacing too aggressive early in runs, low cadence, or signs of overreaching.
4. **Generate a training plan** for the next 4 weeks based on the current fitness level shown in the data. Structure it with easy runs, one tempo session, one interval or fartlek session, and one long run per week.
5. **State what goal you are targeting** (if I have provided one) or ask me for my goal if not yet specified.

Present the output as:
- Data Summary table
- Performance Analysis (bullet points)
- 4-Week Training Plan (week-by-week)
- Coaching Notes
