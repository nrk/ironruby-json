require 'json/common'

module JSON
  # This module holds all the modules/classes that implement JSON's
  # functionality as C extensions.
  module Ext
    #require 'json/ext/parser'
    #require 'json/ext/generator'

    load_assembly 'IronRuby.JsonExt', 'IronRuby.JsonExt'
    $DEBUG and warn "Using c extension for JSON."

    JSON.parser = JSON__::Ext::Parser
    JSON.generator = JSON__::Ext::Generator
  end

  # TODO: this is a terrible hack necessary due to the fact that IronRuby 
  #       can not extend an already defined class in a Ruby module from C#
  remove_const :JSONError
  remove_const :ParserError
  remove_const :NestingError
  remove_const :GeneratorError
  remove_const :CircularDatastructure
  
  const_set :JSONError, JSON__::JSONError
  const_set :ParserError, JSON__::ParserError
  const_set :NestingError, JSON__::NestingError
  const_set :GeneratorError, JSON__::GeneratorError
  const_set :CircularDatastructure, JSON__::CircularDatastructure
end
