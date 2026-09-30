import assert from "node:assert/strict";
import { EditHistory } from "../js/edit-history.mjs";

const snapshot = (dsl, caret = dsl.length) => ({ dsl, selectionStart: caret, selectionEnd: caret, inspector: { screen: 1, panel: 2, line: 3 } });
const original = snapshot('# retained comment\r\n@screen 雪\r\n', 5);
const history = new EditHistory(original);
assert.equal(history.canUndo, false);
history.record(snapshot(original.dsl + "a"), "insertText", 0);
history.record(snapshot(original.dsl + "ab"), "insertText", 100);
assert.deepEqual(history.undo(snapshot(original.dsl + "ab")), original, "One undo removes a typing burst and restores caret, selection and inspector");
const redone = history.redo(original);
assert.equal(redone.dsl, original.dsl + "ab");
redone.inspector.screen = 99;
assert.equal(history.entries[history.index].inspector.screen, 1, "Returned snapshots cannot mutate retained history");

history.record(snapshot(original.dsl + "abc"), "insertText", 1000);
history.record(snapshot(original.dsl + "abc\n@panel next"));
assert.equal(history.undo(snapshot(original.dsl + "abc\n@panel next")).dsl, original.dsl + "abc", "Inspector/palette mutations are atomic");
assert.equal(history.undo(snapshot(original.dsl + "abc")).dsl, original.dsl + "ab", "Typing after a pause is a separate step");
history.record(snapshot(original.dsl + "branch"));
assert.equal(history.canRedo, false, "An edit after undo discards the redo branch");

const moved = snapshot("ab", 1);
const cursorHistory = new EditHistory(snapshot(""));
cursorHistory.record(snapshot("ab"), "insertText", 0);
cursorHistory.capture(moved);
cursorHistory.record(snapshot("axb", 2), "insertText", 100);
assert.deepEqual(cursorHistory.undo(snapshot("axb", 2)), moved, "Moving the caret separates typing groups");

const bounded = new EditHistory(snapshot(""), 3);
for (let i = 1; i <= 10; i++) bounded.record(snapshot(String(i)));
assert.equal(bounded.entries.length, 3);
assert.equal(bounded.undo(snapshot("10")).dsl, "9");
assert.equal(bounded.undo(snapshot("9")).dsl, "8");
assert.equal(bounded.undo(snapshot("8")), null);
bounded.reset(snapshot("new document"));
assert.equal(bounded.canUndo, false, "Loading another document starts a fresh history");
assert.equal(bounded.canRedo, false);
console.log("edit history passed: grouped typing, atomic mutations, selection restoration, branching and bounded storage");
