// Bounded document history shared by typing, palette insertion and inspector edits.
export class EditHistory {
  constructor(initial, limit = 100) {
    this.limit = Math.max(2, limit);
    this.reset(initial);
  }

  reset(snapshot) {
    this.entries = [structuredClone(snapshot)];
    this.index = 0;
    this.group = null;
    this.time = 0;
  }

  get canUndo() { return this.index > 0; }
  get canRedo() { return this.index < this.entries.length - 1; }

  capture(snapshot) {
    const previous = this.entries[this.index];
    if (snapshot.dsl !== previous.dsl) return;
    if (snapshot.selectionStart !== previous.selectionStart || snapshot.selectionEnd !== previous.selectionEnd)
      this.group = null;
    this.entries[this.index] = structuredClone(snapshot);
  }

  record(snapshot, group = null, time = Date.now()) {
    if (snapshot.dsl === this.entries[this.index].dsl) return;
    const merge = group && group === this.group && time - this.time < 750 && this.canUndo && !this.canRedo;
    this.entries.length = this.index + 1;
    if (merge) this.entries[this.index] = structuredClone(snapshot);
    else {
      this.entries.push(structuredClone(snapshot));
      if (this.entries.length > this.limit) this.entries.shift();
      this.index = this.entries.length - 1;
    }
    this.group = group;
    this.time = time;
  }

  undo(current) {
    this.capture(current);
    this.group = null;
    return this.canUndo ? structuredClone(this.entries[--this.index]) : null;
  }

  redo(current) {
    this.capture(current);
    this.group = null;
    return this.canRedo ? structuredClone(this.entries[++this.index]) : null;
  }
}
