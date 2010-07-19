require 'json/common'

module JSON
  # This module holds all the modules/classes that implement JSON's
  # functionality as C extensions.
  module Ext
    #require 'json/ext/parser'
    #require 'json/ext/generator'

    load_assembly 'IronRuby.JsonExt', 'IronRuby.JsonExt'
    $DEBUG and warn "Using c extension for JSON."

    JSON.parser = JSON::Ext::Parser
    JSON.generator = JSON::Ext::Generator
  end
end
