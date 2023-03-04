export {};

import hasWhitespaceAtEitherEnd from "../src/hasWhitespaceAtEitherEnd";

describe("hasWhitespaceAtEitherEnd", () => {
  it("should return true if string has whitespace at either end", () => {
    expect(hasWhitespaceAtEitherEnd(" ")).toBe(true);
    expect(hasWhitespaceAtEitherEnd(" a")).toBe(true);
    expect(hasWhitespaceAtEitherEnd("a ")).toBe(true);
    expect(hasWhitespaceAtEitherEnd(" a ")).toBe(true);
  });

  it("should return false if string does not have whitespace at either end", () => {
    expect(hasWhitespaceAtEitherEnd("a")).toBe(false);
    expect(hasWhitespaceAtEitherEnd("a b")).toBe(false);
    expect(hasWhitespaceAtEitherEnd("ab")).toBe(false);
  });
});
