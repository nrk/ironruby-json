require 'json/common'

module JSON
  # This module holds all the modules/classes that implement JSON's
  # functionality as C extensions.
  module Ext
    #require 'json/ext/parser'
    #require 'json/ext/generator'

    load_assembly 'IronRuby.Libraries.Json', 'IronRuby.Libraries.Json'
    $DEBUG and warn "Using c extension for JSON."
    
    JSON.parser = JSON__::Ext::Parser
    JSON.generator = JSON__::Ext::Generator
  end
end
